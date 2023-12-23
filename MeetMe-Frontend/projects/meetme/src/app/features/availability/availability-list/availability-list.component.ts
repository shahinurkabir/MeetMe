import { NgFor } from '@angular/common';
import { Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { AvailabilityService, IAvailability, ICreateAvailabilityCommand } from '../../../app-core';

@Component({
  selector: 'app-availability-list',
  templateUrl: './availability-list.component.html',
  styleUrls: ['./availability-list.component.scss']
})
export class AvailabilityListComponent implements OnInit, OnDestroy {
  destroyed$:Subject<boolean> = new Subject<boolean>();
  @Output() itemSelected = new EventEmitter<IAvailability>();
  listAvailability: IAvailability[] = [];
  newAvailabilityName: string = "";
  selectedItem_Id: string | undefined;
  @ViewChild(NgForm) frmAddAvailability: NgForm | undefined;
  timeZoneName = Intl.DateTimeFormat().resolvedOptions().timeZone;
  showAddNewModal: boolean = false;
  constructor(
    private availabilityService: AvailabilityService,
    //private modalService: ModalService
  ) {
    this.loadData(undefined);
  }

  ngOnInit(): void {

  }
  onShowAddNewModal() {
    this.resetForm(this.frmAddAvailability);
    this.showAddNewModal = true;
    //this.modalService.open('new-availability-modal')

  }

  onSelectItem(availability?: IAvailability) {
    this.selectedItem_Id = availability?.id;
    this.itemSelected.emit(availability);
  }
  loadData(displayItem_Id: string | undefined) {
    this.availabilityService.getList()
    .pipe(takeUntil(this.destroyed$))
    .subscribe({
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

  onSubmit(frm: NgForm) {

    if (frm.invalid) return;

    let command: ICreateAvailabilityCommand = {
      name: this.newAvailabilityName,
      timeZone: this.timeZoneName
    };

    this.availabilityService.addNew(command)
    .pipe(takeUntil(this.destroyed$))
    .subscribe({
      next: response => {
        this.loadData(response)
      },
      error: (error) => { console.log(error) },
      complete: () => {
        this.newAvailabilityName = "";
        this.showAddNewModal = false;
      }
    });

  }
  onCancelAdd() {
    this.showAddNewModal = false;
  }
  private resetForm(frm: NgForm | undefined) {
    frm?.form.markAsPristine();
    frm?.resetForm();
  }
  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.complete();
  }
}