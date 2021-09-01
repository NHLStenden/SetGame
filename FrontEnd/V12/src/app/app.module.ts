import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BoardComponent } from './components/board/board.component';
import { GUICardComponent } from './components/guicard/guicard.component';
import { SelectionInfoComponent } from './components/selection-info/selection-info.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { HintsComponent } from './components/hints/hints.component';
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent,
    BoardComponent,
    GUICardComponent,
    SelectionInfoComponent,
    StatisticsComponent,
    HintsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
