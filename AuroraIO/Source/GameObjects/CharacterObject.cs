using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.GameObjects {
    class CharacterObject {
        public void equip(int slotID, string resref) {

        }

        public void unequip(int slotID) {

        }

        public void unequip(string resref) {

        }

        public void addFeat(int featID) {

        }

        public void removeFeat(int featID) {

        }

        public void setSkill(int skillID, int skillAmount) {

        }

        public void addSpell(int spellID) {

        }

        public void removeSpell(int spellID) {

        }

        public bool hasSpell(int spellID) {
            return false;
        }

        public void addInventory(string resref, bool droppable) {

        }

        public void removeInventory(string resref) {

        }
        /*
        equip<slot-id> <resref>
    unequip where<predicate>
    add-feat<feat_id>
    rem-feat<feat_id>
    set-skill<skill_id> <skill_amount>

    add-spell<spell_id>
    rem-spell<spell_id>
    has-spell<spell_id>
    add-inv<resref> <droppable>
    mod-inv where <predicate> <droppable>
    rem-inv<resref>
    has-inv<resref>
    */
    }
}
