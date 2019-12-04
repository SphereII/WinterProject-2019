using UnityEngine;
public class DialogActionAddCVar : DialogActionAddBuff
{
    public string strOperator = "set";

    public override void PerformAction(EntityPlayer player)
    {
        if (string.IsNullOrEmpty(Value))
            Value = "1";

        int flValue = 1;
        int.TryParse(Value, out flValue);

        
        if (player.Buffs.HasCustomVar(ID))
        {
            float CurrentValue = player.Buffs.GetCustomVar(ID);
            if (strOperator.ToLower() == "add")
                CurrentValue += flValue;

            if (strOperator.ToLower() == "sub")
                CurrentValue -= flValue;

            if (strOperator.ToLower() == "set")
                CurrentValue = flValue;

            player.Buffs.SetCustomVar(ID, CurrentValue, false);
            return;


        }
    }


}
