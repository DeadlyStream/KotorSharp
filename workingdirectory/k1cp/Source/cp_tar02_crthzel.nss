////////////////////////////////////////////////////////////////////////////////
/*	KOTOR Community Patch
	
	Fired by tar02_zelka021.dlg in tar_m02ac (Taris Upper City South).

	This script is used on Carth's interjections in the Zelka Forn conversation
	to correct his facing. In this case, for those directed toward Zelka.
	
	See also cp_tar02_bastpc, cp_tar02_bastzel, cp_tar02_carthpc.
	
	Issue #78: 
	https://github.com/KOTORCommunityPatches/K1_Community_Patch/issues/78

	DP 2019-09-29                                                             */
////////////////////////////////////////////////////////////////////////////////


void main() {
	
	object oCarth = GetObjectByTag("carth");
	object oZelka = GetObjectByTag("Zelka021", 0);
	
	AssignCommand(oCarth, ActionDoCommand(SetCommandable(TRUE, oCarth)));
	AssignCommand(oCarth, ClearAllActions());
	AssignCommand(oCarth, ActionDoCommand(SetFacingPoint(GetPosition(oZelka))));
}
