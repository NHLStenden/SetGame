# De Front-End voor het spel 'Set'
We gaan in de lessen **Front-end Development** aan de slag met het maken van het spel 'Set'.
Een voorbeeld kun je vinden op deze [link](https://www.setgame.com/set/puzzle).

We gaan dit spel in etappes bouwen, waarbij steeds nieuwe functionaliteit en complexiteit 
toegevoegd wordt.

Er ontstaan grofweg 3 soorten applicaties:
  * Met alleen Javascript functies
  * Met Javascript classes
  * Met het Angular Framework ([link](https://angular.io))
  
De eerste versies (1 t/m 8) maken geen gebruik van een back-end maar produceren zelf hun 
set met kaarten. De versies daarna leunen op de back-end (die je elders in deze GIT repository
kunt vinden).

Het tekenen van de afbeeldingen gebeurt door gebruik te maken van Scalable Vector Graphics
(SVG). Meer hierover kun je vinden op bijv. Mozilla Developer Network (
[MDN SVG](https://developer.mozilla.org/en-US/docs/Web/SVG). 

## Bronnen
Voor het leren over Javascript zijn er diverse goed bronnen. We maken daarbij onderscheid
in referentiemateriaal en tutorials. Voor het referentie materiaal is er natuurlijk altijd 
[Stackoverflow - tag Javascript](https://stackoverflow.com/questions/tagged/javascript).
Als je echter op zoek bent naar de juiste documentatie, dan raad ik persoonlijk aan om gebruik te maken
van Mozilla Developer Network (**MDN**). De formele documentatie staat uiteraard op [W3C](https://www.w3.org/). 
Deze is echter zeer lastig te lezen.

Het voordeel van MDN is ook dat er voorbeelden op staan én er informatie is over of iets werkt in een
bepaalde browser versie ('Compatibiliteit'). 

Een populaire bron is ook [W3Schools](https://www.w3schools.com/). Daar staan ook 'Try now' manieren zodat
je meteen kunt experimenteren. Het is echter mijn ervaring dat er soms niet accurate informatie op staat.

# Zelf proberen
Soms is het handig om gewoon snel aan de slag te kunnen. Dit kan op meerdere online sites, maar twee goede 
zijn [Codepen](https://codepen.io/), [StackBlitz](https://stackblitz.com/). Beide ondersteunen veel frameworks
en bibliotheken zodat je zelfs met NodeJS, Angular, React, Vue en dergelijke aan de slag kunt. 

# Platform voor het serieuze ontwikkelwerk
Voor je project is het echter essentieel dat je op je PC de juiste software installeert. We gaan uiteindelijk aan 
de slag met [Angular](https://angular.io) dus kies een platform dat dit goed ondersteunt. Zaken waar je 
op kunt letten bij de keuze voor een platform:
  * ondersteuning voor code-completion 
    * [Angular](https://angular.io/), 
    * [Typescript 4.x](https://www.typescriptlang.org/), 
    * [javascript](https://developer.mozilla.org/en-US/docs/Web/JavaScript), 
    * [HTML](https://developer.mozilla.org/en-US/docs/Web/HTML), 
    * [CSS](https://developer.mozilla.org/en-US/docs/Web/CSS), [Less](https://lesscss.org/) of [Sass](https://sass-lang.com/)
  * [GIT](https://git-scm.com/) -ondersteuning (optioneel)
  * [Emmet afkortingen](https://emmet.io/)
  * debugger
  * HTML-preview (optioneel)
  * [node](https://nodejs.org/en/) / npm ondersteuning (voor auto-install)
  * ondersteuning voor Angular CLI (componenten maken, [live-server](https://angular.io/cli/serve)) etc.) via de GUI
  * *Darkmodus voor de nachtbrakers.....*
  
 Een platform dat hier uitstekend geschikt voor is, is [WebStorm](https://www.jetbrains.com/webstorm/) van Jetbrains. Deze
 maakt gebruik van Java, maar deze zit in de installatie ingebakken. Je kunt een account aanmaken met je studenten
 account/mail, zodat je gratis van deze software gebruik kunt maken.
 
 Een ander platform is [Visual Studio Code](https://code.visualstudio.com/), maar hier moet je relatief veel plugins nog installeren.
 
 # Front-end in combinatie met Back-end
 Het is mogelijk om je Front-End en Back-End in één platform te ontwikkelen. Zo kun je prima in [Visual Studio](https://visualstudio.microsoft.com/)
 zowel je C# Dapper/Rest API bouwen in hetzelfde project als je Angular Front-End. Let wel op hoe je deze zaken dan goed 
 configureert, compileert en je Live-Servers van Front-End en Back-End aan de praat krijgt. Er zijn in Visual Studio
 kant en klare templates voor! [Voorbeeld](https://marketplace.visualstudio.com/items?itemName=adentum.QuickApp-ASPNETCoreAngularXProjectTemplate)
 
  
 