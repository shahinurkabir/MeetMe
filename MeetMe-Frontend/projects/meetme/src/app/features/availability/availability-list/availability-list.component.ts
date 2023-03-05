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

  onSelectItem(availability: IAvailability) {
    this.itemSelected.emit(availability);
  }
  loadList() {
    this.availabilityService.getList().subscribe({
      next: response => {
        this.listAvailability = response;
        if (response.length>0){
          this.onSelectItem(response[0]);
        }
      },
      error: (error) => {
        console.log(error);
        alert(error);
      }
    })

  }
  
  onAddNew() {
    let offset = new Date().getTimezoneOffset() / -60;
    let command: CreateAvailabilityCommand = {
      name: this.newAvailabilityName,
      timeZoneOffset: offset
    };

    this.availabilityService.addNew(command).subscribe({
      next: response => {
        this.loadList()
      },
      error: (error) => { console.log(error) },
      complete: () => { this.modalService.close() }
    });

  }
}