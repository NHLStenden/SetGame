export class HintGUI {
    htmlID;

    constructor(htmlID) {
        this.htmlID = htmlID;
    }


    clear() {
        const hintsShapeDiv = document.querySelector(`#${this.htmlID}`);
        hintsShapeDiv.innerHTML = '';
    }


    showHintsAsCards(hints) {
        this.clear();
        const hintsShapeDiv = document.querySelector(`#${this.htmlID}`);
        for(const hint of hints){
            let hintCard = '';

            for(const card of hint.sort((a,b) => a.nrOfShapes - b.nrOfShapes)){
                let cardShapes = '';
                for (let s = 0; s < card.nrOfShapes; s++) {
                    const oneShape = `<div>
                    <svg viewBox="0 0 150 120" width="50px" height="40px">
                        <use xlink:href="#svg_defs"></use>
                        <use xlink:href="#card_${card.shape}" class="shape ${card.fill} ${card.color}"></use>
                    </svg>
                </div>`;
                    cardShapes += oneShape;
                }

                hintCard += `<div class="hintcard">${cardShapes}</div>`;
            }
            hintsShapeDiv.innerHTML += `<div class="oneHint">${hintCard}</div>` ;
        }
    }// showHintsAsCards

}