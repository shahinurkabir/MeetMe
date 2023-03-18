import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CloneAvailabilityCommand } from '../../../commands/cloneAvailabilityCommand';
import { CreateAvailabilityCommand } from '../../../commands/createAvailabilityCommand';
import { EditAvailabilityNameCommand } from '../../../commands/editAvailabilityNameCommand';
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

  constructor(
    private availabilityService: AvailabilityService,
    private modalService: ModalService
  ) {
    this.loadList();
  }

  ngOnInit(): void {

  }
  onShowAddNewModal() {
    this.modalService.open('new-availability-modal')

  }

  onSelectItem(availability?: IAvailability) {
    this.selectedItem_Id = availability?.id;
    this.itemSelected.emit(availability);
  }
  loadList() {
    this.availabilityService.getList().subscribe({
      next: response => {
        this.listAvailability = response;
        this.selectedItem()
      },
      error: (error) => {
        console.log(error);
        alert(error);
      }
    })

  }

  selectedItem() {
    if (this.listAvailability.length == 0) {
      this.onSelectItem(undefined)
      return;
    }
    if (this.selectedItem_Id != undefined) {
      let itemToSelect = this.listAvailability.find(e => e.id);
      if (itemToSelect) {
        this.onSelectItem(itemToSelect)
        return;
      }
    }
    this.onSelectItem(this.listAvailability[0]);
  }

  onAddNew() {
    let offset = new Date().getTimezoneOffset() / -60;
    let command: CreateAvailabilityCommand = {
      name: this.newAvailabilityName,
      timeZoneOffset: offset
    };

    this.availabilityService.addNew(command).subscribe({
      next: response => {
        this.selectedItem_Id = response;
        this.loadList()
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
}