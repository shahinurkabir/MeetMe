import { Component, OnInit } from '@angular/core';
import { ICard } from '../../interfaces/ICard';
import { ICardContainer } from '../../interfaces/ICardContainer';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.scss']
})
export class AddressComponent implements OnInit, ICard {
  cardContainer: ICardContainer | undefined;
  constructor() { }

  updateUI(cardContainer: ICardContainer): void {
    this.cardContainer = cardContainer;
  }
  
  updateModel(): void {
    let addressModel=this.cardContainer?.getModel().addressModel!;
    addressModel.city="Dinajpur"
  }
  
  isValid(): boolean {
    return false;
  }

  ngOnInit(): void {
  }

}
