using UnityEngine;
public class DialogRequirementHasCVarSDX : BaseDialogRequirement
{
    private static string AdvFeatureClass = "AdvancedDialogDebugging";

    public string strOperator = "eq";
    public override bool CheckRequirement(EntityPlayer player)
    {
        if (player.Buffs.HasCustomVar(ID))
        {
            // If value is not specified, accepted it.
            if (string.IsNullOrEmpty(Value))
                return true;

            float flValue = 0f;
            float.TryParse(Value, out flValue);
            float flPlayerValue = player.Buffs.GetCustomVar(ID);
            AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: " + ID + " Value: " + flValue + " Player Value: " + flPlayerValue + " Operator: " + strOperator);

     
            if (strOperator.ToLower() == "lt")
            {
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Checking for LT: " + flPlayerValue + " < " + flValue);
                if (flPlayerValue < flValue)
                    return false;
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Failed for LT: " + flPlayerValue + " < " + flValue);
            }
            else if (strOperator.ToLower() == "gt")
            {
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Checking for GT: " + flPlayerValue + " > " + flValue);
                if (flPlayerValue > flValue)
                    return false;
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Failed for GT: " + flPlayerValue + " > " + flValue);
            }
            else if (strOperator.ToLower() == "gte")
            {
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Checking for GTE: " + flPlayerValue + " >= " + flValue);
                if (flPlayerValue >= flValue)
                    return false;
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Failed for GTE: " + flPlayerValue + " >= " + flValue);
            }
            else if (strOperator.ToLower() == "lte")
            {
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Checking for LTE: " + flPlayerValue + " <= " + flValue);
                if (flPlayerValue <= flValue)
                    return false;
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Failed for GT: " + flPlayerValue + " > " + flValue);
            }
            else
            {
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Checking for equality: " + flPlayerValue + " == " + flValue);
                if (flValue == flPlayerValue)
                    return false;
                AdvLogging.DisplayLog(AdvFeatureClass, "HasCvar: Failed for equality: " + flPlayerValue + " == " + flValue);
            }

            AdvLogging.DisplayLog(AdvFeatureClass, "Has CVar: " + ID + "  Value: " + flValue + " Player Value: " + flPlayerValue + " Operator: " + strOperator + " :: No Result");
           
        }

        // If the Cvar does not exist, but does have a value to be checked, pass the condition. It just may not be set yet.
        //if (!string.IsNullOrEmpty(Value))
        //{
        //    AdvLogging.DisplayLog(AdvFeatureClass, "HasCVar: " + ID + " Player does not have this cvar. Returning true.");
        //    return true;
        //}
        AdvLogging.DisplayLog(AdvFeatureClass, "Has CVar:: false");
        return true;
    }

}


