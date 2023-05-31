import { AfterViewInit, Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ICard } from '../interfaces/ICard';
import { ContactModel } from '../models/models';
import { ICardContainer } from '../interfaces/ICardContainer';

@Component({
  selector: 'app-distribution',
  templateUrl: './distribution.component.html',
  styleUrls: ['./distribution.component.scss']
})
export class DistributionComponent implements OnInit, AfterViewInit, ICardContainer {

  @ViewChildren("card") cards: QueryList<ICard> | undefined
  contactModel: ContactModel = {
    nameModel: { firstName: "Shahin", lastName: "Kabir" },
    addressModel: { city: "Dhaka", state: "Dhaka", zip: "123" }
  }
  constructor() { }

  ngAfterViewInit(): void {
    this.bindCards();
  }

  getModel(): ContactModel {
    return this.contactModel;
  }

  ngOnInit(): void {
  }

  bindCards() {
    this.cards?.forEach(card => {
      card.updateUI(this)
    })
  }

  saveData() {

    if (!this.isValid()) return;

    console.log("Validation Success")

    this.cards?.forEach(card => card.updateModel())

  }

  isValid(): boolean {
    let result: boolean = true;
    try {
      this.cards?.forEach(card => {
        if (!card.isValid()) throw new Error("Invalid");
      })
    }
    catch (error) {
      result = false;
    }


    return result;
  }

}
