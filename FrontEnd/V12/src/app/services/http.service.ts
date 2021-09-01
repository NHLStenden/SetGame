import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {Card} from "../models/card";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  baseURL: string;

  constructor(private http: HttpClient) {
    this.baseURL = environment.BaseURL;
  }

  getRequest<T>(url: string): Observable<T>  {
    return this.http.get<T>(this.baseURL + url);
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
  getDeckById(id: string): Observable<Number>  {
    return this.getRequest<Number>(`/Deck/GetDeckById?deckId=${id}`);
  }

  /**
   * Gets a card from the deck through HTTP backend and converts the result into a Card-class
   * @returns Promise
   *
   */
  getOneCardFromDeck(id: number): Observable<Card> {
    return this.getRequest<Card>(`/Deck/GetNextCardFromDeck?deckId=${id}`)
      .pipe(
        map(data => new Card(data.fill, data.color, data.shape, data.nrOfShapes))
      );
  }

}
