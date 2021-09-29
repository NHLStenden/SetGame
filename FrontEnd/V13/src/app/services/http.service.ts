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
    return this.getRequest('/Game/StartNewGame/1');
  }

  /**
   * @returns Promise
   *
   */
  getDeckById(id: string): Observable<Number>  {
    return this.getRequest<Number>(`/Game/${id}`);
  }

  /**
   * Gets a card from the deck through HTTP backend and converts the result into a Card-class
   * @returns Promise
   *
   */
  getOneCardFromDeck(id: number): Observable<Card> {
    return this.getRequest<Card[]>(`/Game/DrawCards/${id}?numberOfCards=1`)
      .pipe(
        map(data => {
            let card = data[0];
            return new Card(card.fill, card.color, card.shape, card.nrOfShapes)
          }

        )
      );
  }

}
