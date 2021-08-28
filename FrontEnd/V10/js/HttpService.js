import {Card} from "./Card.js";

export class HttpService {
    baseURL;
    constructor(baseURL) {
        this.baseURL = baseURL;
    }

    getRequest(url)  {
        return new Promise(((resolve, reject) => {
            const request = new XMLHttpRequest();
            request.open('get', this.baseURL + url);
            request.onload = () => {
                if (request.status === 200) {
                    resolve(JSON.parse( request.response) );
                }
                else {
                    reject({status: request.status, info: request.statusText});
                }
            };
            request.onerror = () => {
                reject({status: request.status, info: request.statusText});
            };

            request.send();

        }));
    }

    /**
     * @returns Promise
     *
     */
    getNewDeck()  {
        return this.getRequest('/Deck/GetNewDeck');
    }

    /**
     * @returns Promise
     *
     */
    getDeckById(id)  {
        return this.getRequest(`/Deck/GetDeckById?deckId=${id}`);
    }

    /**
     * Gets a card from the deck through HTTP backend and converts the result into a Card-class
     * @returns Promise
     *
     */
    getOneCardFromDeck(id)  {
        return new Promise((resolve, reject) => {
            this.getRequest(`/Deck/GetNextCardFromDeck?deckId=${id}`).then(
                (data) => {
                    const card = new Card(data.color, data.fill, data.shape, data.nrOfShapes);
                    resolve(card);
                },
                (error) => reject(error)
            );
        });

    }

}