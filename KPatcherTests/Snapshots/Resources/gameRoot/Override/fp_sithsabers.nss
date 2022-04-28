#include "k_inc_force"


int FORCE_POWER_SITH_SABERS = #2DAMEMORY45#;


void main()  {





if (!GetIsObjectValid(GetObjectByTag("n_sithsabers"))) 
{ 
SignalEvent(OBJECT_SELF, EventSpellCastAt(OBJECT_SELF, GetSpellId(), FALSE));
object oNPC = CreateObject( OBJECT_TYPE_CREATURE,"n_sithsabers", GetLocation(GetFirstPC())); 
DelayCommand(30.0, DestroyObject(oNPC)); 
 

} 
}
