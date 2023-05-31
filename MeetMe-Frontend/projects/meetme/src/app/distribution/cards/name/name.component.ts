import { Component, OnInit } from '@angular/core';
import { ICard } from '../../interfaces/ICard';
import { ICardContainer } from '../../interfaces/ICardContainer';
import { ContactModel } from '../../models/models';

@Component({
  selector: 'app-name',
  templateUrl: './name.component.html',
  styleUrls: ['./name.component.scss']
})
export class NameComponent implements OnInit, ICard {
  cardContainer: ICardContainer | undefined;

  constructor() { }
  
  updateUI(cardContainer: ICardContainer): void {
    this.cardContainer = cardContainer;
  }
  
  updateModel(): void {
    let nameModel = this.cardContainer?.getModel().nameModel!;
    nameModel.firstName = "ABC";
  }
  
  isValid(): boolean {
    return true;
  }
  ngOnInit(): void {
  }

}
