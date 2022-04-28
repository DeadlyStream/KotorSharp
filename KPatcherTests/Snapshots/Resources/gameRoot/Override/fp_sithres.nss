#include "k_inc_force"


int FORCE_POWER_SITH_RESURRECTION = #2DAMEMORY43#;


void main()  {





if (!GetIsObjectValid(GetObjectByTag("n_sithghost"))) 
{ 
SignalEvent(OBJECT_SELF, EventSpellCastAt(OBJECT_SELF, GetSpellId(), FALSE));
object oNPC = CreateObject( OBJECT_TYPE_CREATURE,"n_sithghost", GetLocation(GetFirstPC())); 
DelayCommand(30.0, DestroyObject(oNPC)); 
effect eVis2 = EffectVisualEffect( VFX_FNF_GRENADE_POISON ); 
ApplyEffectAtLocation(DURATION_TYPE_INSTANT, eVis2, GetLocation(oNPC)); 
} 
}
