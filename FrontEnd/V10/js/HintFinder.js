import {SelectionList} from "./SelectionList.js";

export class HintFinder {

    constructor() {
    }

    getHintsFromCards(/* CardList */ cardList) {
        const setsFound = this.findAllSets(cardList, []);
        return setsFound;
    }

    /**
     * Find all possible sets by using backtracking.
     * @param cards the set of cards to investigate
     * @param currentSet the current set under investigation
     * @return {[]|*[]} an array comprising of valid sets
     */

     findAllSets(cards, currentSet){

        // for this level, clear the sets found
        const setsFound = [];

        // make a collection of cards not currently in the set 'currentSet' so we know which cards remain
        // to be investigated
        const otherCards = cards.filter(c => currentSet.find(c2 => c2.id === c.id) === undefined);

        // do we have a set of 3 cards? if not, keep collecting; if yes, check if it is a set.
        if (currentSet.length < 3) {

            // check if there are other cards left to be investigated
            if (otherCards.length >0){
                // cycle through all the 'other cards' using a for-loop on this level.
                for (let i = 0; i < otherCards.length-1; i++) {
                    // get a new card
                    const newcard = otherCards[i];

                    // if we remove this card, is there anything left to check?
                    // because all the cards "on the left of the card taken" do not need to be investigated
                    // again
                    let remainingToCheck;
                    if (otherCards.length > 1) {
                        // construct a set of cards to be investigated in the next recursive call by
                        // making a copy of a slice of the cards "on the right of the taken card".
                        remainingToCheck = otherCards.slice(i+1);
                    }
                    else{
                        remainingToCheck = [];
                    }
                    // start recursing the remaining cards. first extend the current set with the new
                    // card taken, and construct a new set for the next level.
                    const setsFoundBelow = this.findAllSets(remainingToCheck, [...currentSet, newcard]);

                    // add the sets found on the lower levels to the sets found.
                    setsFound.push(...setsFoundBelow);
                }
            }
        }
        else {
            const list = new SelectionList();

            list.addMultiple(currentSet);

            if (list.checkIfSelectionIsASet().isItASet) {
                // return values must always be an array, so surround the result in brackets [] to construct
                // an array with one value.
                return [currentSet];
            }
        }

        return setsFound;
    }// findAllSet

}