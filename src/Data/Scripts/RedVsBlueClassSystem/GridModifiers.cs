using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Utils;

namespace RedVsBlueClassSystem
{
    public class GridModifiers
    {
        public float ThrusterForce = 1;
        public float ThrusterEfficiency = 1;
        public float GyroForce = 1;
        public float GyroEfficiency = 1;
        public float RefineEfficiency = 1;
        public float RefineSpeed = 1;
        public float RefinePowerEfficiency = 1;
        public float AssemblerSpeed = 1;
        public float AssemblerPowerEfficiency = 1;
        public float PowerProducersOutput = 1;
        public float DrillHarvestMutiplier = 1;
        public float DamageModifier = 1;
        public float BulletDamageModifier = 1;
        public float DeformationDamageModifier = 1;
        public float ExplosionDamageModifier = 1;

        public override string ToString()
        {
            return $"<GridModifiers ThrusterForce={ThrusterForce} ThrusterEfficiency={ThrusterEfficiency} GyroForce={GyroForce} GyroEfficiency={GyroEfficiency} RefineEfficiency={RefineEfficiency} RefineSpeed={RefineSpeed} RefinePowerEfficiency={RefinePowerEfficiency} AssemblerSpeed={AssemblerSpeed} AssemblerPowerEfficiency={AssemblerPowerEfficiency} PowerProducersOutput={PowerProducersOutput} DrillHarvestMutiplier={DrillHarvestMutiplier} DamageModifier={DamageModifier} BulletDamageModifier={BulletDamageModifier} DeformationDamageModifier={DeformationDamageModifier} ExplosionDamageModifier={ExplosionDamageModifier} />";
        }

        public IEnumerable<ModifierNameValue> GetModifierValues()
        {
            yield return new ModifierNameValue("Thruster force", ThrusterForce);
            yield return new ModifierNameValue("Thruster efficiency", ThrusterEfficiency);
            yield return new ModifierNameValue("Gyro force", GyroForce);
            yield return new ModifierNameValue("Gryo efficiency", GyroEfficiency);
            yield return new ModifierNameValue("Refinery efficiency", RefineEfficiency);
            yield return new ModifierNameValue("Refinery speed", RefineSpeed);
            yield return new ModifierNameValue("Refinery power efficiency", RefinePowerEfficiency);
            yield return new ModifierNameValue("Assembler speed", AssemblerSpeed);
            yield return new ModifierNameValue("Assembler power efficiency", AssemblerPowerEfficiency);
            yield return new ModifierNameValue("Power output", PowerProducersOutput);
            yield return new ModifierNameValue("Drill harvest", DrillHarvestMutiplier);
            yield return new ModifierNameValue("Damage modifier", DamageModifier);
            yield return new ModifierNameValue("Bullet damage modifier", BulletDamageModifier);
            yield return new ModifierNameValue("Deformation damage modifier", DeformationDamageModifier);
            yield return new ModifierNameValue("Explosion damage modifier", ExplosionDamageModifier);
        }

        public float GetDamageModifier(MyStringHash type)
        {
            if (type == MyDamageType.Bullet)
            {
                return DamageModifier * BulletDamageModifier;
            }

            if (type == MyDamageType.Explosion)
            {
                return DamageModifier * ExplosionDamageModifier;
            }

            if (type == MyDamageType.Deformation)
            {
                return DamageModifier * DeformationDamageModifier;
            }

            return DamageModifier;
        }
    }

    public struct ModifierNameValue
    {
        public string Name;
        public float Value;

        public ModifierNameValue(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}
