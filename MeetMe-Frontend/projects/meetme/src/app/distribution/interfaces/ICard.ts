import { ICardContainer } from "./ICardContainer";

export interface ICard {
  isValid():boolean;   
  updateUI(cardContainer:ICardContainer):void;
  updateModel():void
}