import {EventEmitter, Injectable} from '@angular/core';
import {HttpService} from "./http.service";
import {CardList} from "../models/card-list";
import {forkJoin, Observable} from "rxjs";
import {Card} from "../models/card";
import {SelectionList} from "../models/selection-list";
import {SetCheckResult} from "../models/set-check-result";

@Injectable({
  providedIn: 'root'
})
export class BoardControllerService {

  public cards: CardList;
  public selectedCards: SelectionList;
  public idDeck: number;

  public selectionChanged$: EventEmitter<void>;
  public checkedForSelection$: EventEmitter<SetCheckResult>;

  selectionInfo: SetCheckResult | undefined;

  constructor(private http: HttpService) {
    this.cards = new CardList();
    this.selectedCards = new SelectionList();
    this.idDeck = -1;
    this.selectionChanged$ = new EventEmitter<void>();
    this.checkedForSelection$ = new EventEmitter<SetCheckResult>();

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

  selectCard(card: Card): boolean {
    let wasSelected = false;
    console.log(card);

    if (this.selectedCards.length < 3) {
      this.selectedCards.add(card);
      wasSelected = true;
      this.clearSelectionInfo();
    }
    if (this.selectedCards.length === 3) {
      this.checkForSet();
    }
    this.selectionChanged$.emit();
    return wasSelected;
  }// selectCard

  clearSelectionInfo(): void {
    this.selectionInfo = undefined;
    this.informObserversOfSetCheck();
  }

  unselectCard(card: Card): void {
    this.selectedCards.removeCard(card);
    this.clearSelectionInfo();
    this.selectionChanged$.emit();
  }

  checkForSet(): void {
    this.selectedCards.checkIfSelectionIsASet();
    if (this.selectedCards.length === 3) {
      this.selectionInfo = this.selectedCards?.checkIfSelectionIsASet();
    }
    else {
      this.selectionInfo = undefined;
    }
    this.informObserversOfSetCheck();
  }

  informObserversOfSetCheck(): void {
    this.checkedForSelection$.emit(this.selectionInfo);
  }
}
