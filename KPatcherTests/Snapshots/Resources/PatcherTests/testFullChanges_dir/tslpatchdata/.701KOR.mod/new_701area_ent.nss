void sub1(string stringParam1) { 
int int1 = 0; 
object object1 = GetObjectByTag(stringParam1, int1); 
while (GetIsObjectValid(object1)) { 
DestroyObject(object1, 0.0, 1, 0.0, 0); 
object1 = GetObjectByTag(stringParam1, (int1++)); 
} 
} 
void main() { 

if (GetJournalEntry("korrfloor") >= 20){
sub1("doorinvblk"); 

CreateObject(OBJECT_TYPE_WAYPOINT, "wp_valpath", Location(Vector(49.0, 265.0, 1.6), 0.0));

}

ExecuteScript("k_701area_enter", OBJECT_SELF);




}
