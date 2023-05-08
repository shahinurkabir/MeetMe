import { NgFor } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CreateAvailabilityCommand } from '../../../commands/createAvailabilityCommand';
import { ModalService } from '../../../controls/modal/modalService';
import { IAvailability } from '../../../models/IAvailability';
import { AvailabilityService } from '../../../services/availability.service';

@Component({
  selector: 'app-availability-list',
  templateUrl: './availability-list.component.html',
  styleUrls: ['./availability-list.component.scss']
})
export class AvailabilityListComponent implements OnInit {
  @Output() itemSelected = new EventEmitter<IAvailability>();
  listAvailability: IAvailability[] = [];
  newAvailabilityName: string = "";
  selectedItem_Id: string | undefined;
  @ViewChild(NgForm) frmAddAvailability: NgForm | undefined;

  constructor(
    private availabilityService: AvailabilityService,
    private modalService: ModalService
  ) {
    this.loadData(undefined);
  }

  ngOnInit(): void {

  }
  onShowAddNewModal() {
    this.resetForm(this.frmAddAvailability);
    this.modalService.open('new-availability-modal')

  }

  onSelectItem(availability?: IAvailability) {
    this.selectedItem_Id = availability?.id;
    this.itemSelected.emit(availability);
  }
  loadData(displayItem_Id: string | undefined) {
    this.availabilityService.getList().subscribe({
      next: response => {
        this.listAvailability = response;
        this.setDisplayItem(displayItem_Id)
      },
      error: (error) => {
        console.log(error);
        alert(error);
      }
    })

  }

  setDisplayItem(selectedItem: string | undefined) {
    let itemToDisplay: IAvailability | undefined;
    if (selectedItem != undefined) {
      itemToDisplay = this.listAvailability.find(e => e.id == selectedItem);
    }
    if (itemToDisplay == undefined && this.listAvailability.length > 0) {
      itemToDisplay = this.listAvailability[0];
    }
    this.onSelectItem(itemToDisplay);
  }

  onAddNew(e: any) {
    this.frmAddAvailability?.onSubmit(e);

  }
  onSubmit(frm: NgForm) {

    if (frm.invalid) return;

    let offset = new Date().getTimezoneOffset() / -60;
    let command: CreateAvailabilityCommand = {
      name: this.newAvailabilityName,
      timeZoneOffset: offset
    };

    this.availabilityService.addNew(command).subscribe({
      next: response => {
        //this.selectedItem_Id = response;
        this.loadData(response)
      },
      error: (error) => { console.log(error) },
      complete: () => {
        this.newAvailabilityName = "";
        this.modalService.close()
      }
    });

  }
  onCancelAdd() {
    this.modalService.close();
  }
  private resetForm(frm: NgForm | undefined) {
    frm?.form.markAsPristine();
    frm?.resetForm();
  }
}