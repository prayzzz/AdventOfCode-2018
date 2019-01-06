using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day24 : IDay
    {
        private const bool WithLogging = false;
        private static readonly Regex UnitsPattern = new Regex("(?<count>\\d+) units");
        private static readonly Regex HpPattern = new Regex("(?<count>\\d+) hit");
        private static readonly Regex WeakPattern = new Regex("weak to (?<elements>\\w+(?:, \\w+)*)");
        private static readonly Regex ImmunePattern = new Regex("immune to (?<elements>\\w+(?:, \\w+)*)");
        private static readonly Regex DamagePattern = new Regex("(?<count>\\d+) (?<element>\\w+) damage");
        private static readonly Regex InitiativePattern = new Regex("initiative (?<count>\\d+)");

        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 26343)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 5549)");
        }

        private static int SolvePart1(string input)
        {
            var immuneSystem = new List<UnitGroup>();
            var infection = new List<UnitGroup>();

            ParseInput(input, immuneSystem, infection);

            Battle(immuneSystem, infection);

            if (immuneSystem.Any() && !infection.Any())
            {
                return immuneSystem.Sum(g => g.Count);
            }

            if (infection.Any() && !immuneSystem.Any())
            {
                return infection.Sum(g => g.Count);
            }

            return -1;
        }

        private static int SolvePart2(string input)
        {
            var immuneSystem = new List<UnitGroup>();
            var infection = new List<UnitGroup>();

            ParseInput(input, immuneSystem, infection);

            for (var boost = 0; boost < int.MaxValue; boost++)
            {
                var immuneCur = immuneSystem.Select(i => (UnitGroup) i.Clone()).ToList();
                immuneCur.ForEach(g => g.Damage += boost);
                var infectionCur = infection.Select(i => (UnitGroup) i.Clone()).ToList();

                Battle(immuneCur, infectionCur);

                if (immuneCur.Any() && !infectionCur.Any())
                {
                    return immuneCur.Sum(g => g.Count);
                }
            }

            return -1;
        }

        private static void Battle(List<UnitGroup> immuneCur, List<UnitGroup> infectionCur)
        {
            while (immuneCur.Any() && infectionCur.Any())
            {
                immuneCur.ForEach(g => LogLine($"{g.Type} Group {g.Id} contains {g.Count} Units"));
                infectionCur.ForEach(g => LogLine($"{g.Type} Group {g.Id} contains {g.Count} Units"));
                LogLine();

                // Select Targets
                var attackerToDefender = new Dictionary<UnitGroup, UnitGroup>();
                SelectTargets(immuneCur, infectionCur, attackerToDefender);
                SelectTargets(infectionCur, immuneCur, attackerToDefender);
                LogLine();

                // Attack
                var summedKills = 0;
                foreach (var (attacker, defender) in attackerToDefender.OrderByDescending(p => p.Key.Initiative))
                {
                    if (defender.Count < 1 || attacker.Count < 1)
                    {
                        continue;
                    }

                    var damage = CalculateDamage(attacker, defender);
                    var killedUnits = Math.Min(defender.Count, damage / defender.HealthPoints);
                    defender.Count -= killedUnits;
                    summedKills += killedUnits;

                    LogLine($"{attacker.Type} Group {attacker.Id} attacks {defender.Type} group {defender.Id}, killing {killedUnits}");
                }

                immuneCur.RemoveAll(g => g.Count < 1);
                infectionCur.RemoveAll(g => g.Count < 1);

                if (summedKills == 0)
                {
                    break;
                }

                LogLine();
                LogLine("#######################################################");
                LogLine();
            }
        }

        private static void ParseInput(string input, List<UnitGroup> immuneSystem, List<UnitGroup> infection)
        {
            var id = 0;
            var currentType = "";
            List<UnitGroup> currentGroup = null;

            var lines = input.Split("\n").Select(l => l.Trim()).ToList();
            foreach (var line in lines)
            {
                id++;
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.Contains("Immune System"))
                {
                    currentGroup = immuneSystem;
                    currentType = "Immune";
                    id = 0;
                    continue;
                }

                if (line.Contains("Infection"))
                {
                    currentGroup = infection;
                    currentType = "Infection";
                    id = 0;
                    continue;
                }

                var units = int.Parse(UnitsPattern.Match(line).Groups["count"].Value);
                var hp = int.Parse(HpPattern.Match(line).Groups["count"].Value);
                var damage = int.Parse(DamagePattern.Match(line).Groups["count"].Value);
                var init = int.Parse(InitiativePattern.Match(line).Groups["count"].Value);

                var damageElement = DamagePattern.Match(line).Groups["element"].Value.Trim();
                var weaknesses = WeakPattern.Match(line).Groups["elements"].Value
                                            .Split(",")
                                            .Where(w => !string.IsNullOrEmpty(w))
                                            .Select(w => w.Trim())
                                            .ToList();

                var immunities = ImmunePattern.Match(line).Groups["elements"].Value
                                              .Split(",")
                                              .Where(w => !string.IsNullOrEmpty(w))
                                              .Select(i => i.Trim())
                                              .ToList();

                currentGroup?.Add(new UnitGroup(id, currentType, units, hp, immunities, weaknesses, damage, damageElement, init));
            }
        }

        private static void SelectTargets(IEnumerable<UnitGroup> attackers,
                                          IEnumerable<UnitGroup> defenders,
                                          IDictionary<UnitGroup, UnitGroup> attackerToDefender)
        {
            var availableDefenders = defenders.ToList();
            foreach (var attacker in attackers.OrderByDescending(g => g.EffectivePower).ThenByDescending(g => g.Initiative))
            {
                var possibleDefenders = availableDefenders.Select(d => (d, Damage: CalculateDamage(attacker, d), d.EffectivePower, d.Initiative))
                                                          .Where(d => d.Damage > 0)
                                                          .OrderByDescending(d => d.Damage)
                                                          .ThenByDescending(d => d.EffectivePower)
                                                          .ThenByDescending(d => d.Initiative)
                                                          .ToList();

                possibleDefenders.ForEach(tuple =>
                {
                    var (defender, damage, _, _) = tuple;
                    LogLine($"{attacker.Type} Group {attacker.Id} would deal {defender.Type} group {defender.Id} {damage} damage");
                });

                var selectedDefender = possibleDefenders.Select(x => x.d).FirstOrDefault();
                if (selectedDefender != null)
                {
                    attackerToDefender.Add(attacker, selectedDefender);
                    availableDefenders.Remove(selectedDefender);
                }
            }
        }

        private static int CalculateDamage(UnitGroup attacker, UnitGroup defender)
        {
            if (defender.Immunities.Contains(attacker.DamageElement))
            {
                return 0;
            }

            if (defender.Weakness.Contains(attacker.DamageElement))
            {
                return 2 * attacker.EffectivePower;
            }

            return attacker.EffectivePower;
        }

        private static void LogLine(string value = "")
        {
            if (WithLogging)
            {
                Console.WriteLine(value);
            }
        }

        [DebuggerDisplay("Group: {Id}, Count: {Count}")]
        private class UnitGroup : ICloneable
        {
            public UnitGroup(int id,
                             string type,
                             int count,
                             int healthPoints,
                             List<string> immunities,
                             List<string> weakness,
                             int damage,
                             string damageElement,
                             int initiative)
            {
                Id = id;
                Type = type;
                Count = count;
                HealthPoints = healthPoints;
                Immunities = immunities;
                Weakness = weakness;
                Damage = damage;
                DamageElement = damageElement;
                Initiative = initiative;
            }

            public int Id { get; }
            public string Type { get; }
            public int Count { get; set; }
            public int HealthPoints { get; }
            public List<string> Immunities { get; }
            public List<string> Weakness { get; }
            public int Damage { get; set; }
            public string DamageElement { get; }
            public int Initiative { get; }
            public int EffectivePower => Count * Damage;

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}