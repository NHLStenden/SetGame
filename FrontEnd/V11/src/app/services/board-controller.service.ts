import { Injectable } from '@angular/core';
import {HttpService} from "./http.service";
import {CardList} from "../models/card-list";
import {forkJoin, Observable} from "rxjs";
import {Card} from "../models/card";

@Injectable({
  providedIn: 'root'
})
export class BoardControllerService {

  public cards: CardList;
  public idDeck: number;

  constructor(private http: HttpService) {
    this.cards = new CardList();
    this.idDeck = -1;

    this.http.getNewDeck().subscribe( (data: any) => {
      this.idDeck = data.deckId;
      this.addCardsToBoard(12);
    });
  }// constructor

  private getCards(nrOfCardsToGet: number): Observable<CardList> {
    const requests : Observable<Card>[] = [];
    for (let i = 0; i < nrOfCardsToGet; i++) {
      requests.push( this.http.getOneCardFromDeck(this.idDeck) );
    }
    return new Observable<CardList>(subscriber => {
      forkJoin(requests).subscribe((data: Card[]) => {
        const cards = new CardList();
        for (const card of data) {
          cards.add(card);
        }
        subscriber.next(cards);
      });
    });
  }// getCards

  addCardsToBoard(nrOfCardsToGet: number): void {
    this.getCards(nrOfCardsToGet).subscribe(
      cardList => {
        console.log('Cards fetched');
        this.cards.addMultiple(cardList.cards);
      }
    );
  }
}
