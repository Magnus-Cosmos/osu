// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Rulesets.Osu.Difficulty.Preprocessing;

namespace osu.Game.Rulesets.Osu.Difficulty.Skills
{
    /// <summary>
    /// Represents the skill required to press keys with regards to keeping up with the speed at which objects need to be hit.
    /// </summary>
    public class Speed : Skill
    {
        protected override double SkillMultiplier => 1400;
        protected override double StrainDecayBase => 0.3;

        private const double single_spacing_threshold = 125;
        private const double stream_spacing_threshold = 110;
        private const double almost_diameter = 90;

        protected override double StrainValueOf(OsuDifficultyHitObject current)
        {
            double distance = current.Distance;
            double speedBonus = 1.0;

            if (current.DeltaTime < 68) // 68 = 220 BPM 1/4th snapping in MS.
            {
                speedBonus = 68 / current.DeltaTime - 1.0;
            }
            else
                speedBonus = 0;

            double speedValue;
            if (distance > single_spacing_threshold)
            {
                speedValue = 2.5;
                speedValue *= (1.0 + 1.2 * speedBonus);
            }
            else if (distance > stream_spacing_threshold)
            {
                speedValue = 1.6 + 0.9 * (distance - stream_spacing_threshold) / (single_spacing_threshold - stream_spacing_threshold);
                speedValue *= (1.0 + speedBonus);
            }
            else if (distance > almost_diameter)
            {
                speedValue = 1.2 + 0.4 * (distance - almost_diameter) / (stream_spacing_threshold - almost_diameter);
                speedValue *= (1.0 + 0.8 * speedBonus);
            }
            else if (distance > almost_diameter / 2)
            {
                speedValue = 0.95 + 0.25 * (distance - almost_diameter / 2) / (almost_diameter / 2);
                speedValue *= (1.0 + 0.5 * speedBonus);
            }
            else
                speedValue = 0.95;

            return speedValue / current.DeltaTime;
        }
    }
}
