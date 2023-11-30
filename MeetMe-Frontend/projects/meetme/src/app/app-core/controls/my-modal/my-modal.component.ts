import { Component, ContentChild, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-my-modal',
  templateUrl: './my-modal.component.html',
  styleUrls: ['./my-modal.component.scss']
})
export class MyModalComponent implements OnInit {

  @Input() isVisible: boolean = false;
  @Input() modalWidth: string = '400px';
  @Input() modalHeight: string = 'auto';
  @Input() modalTitle: string = 'Modal Title';
  @Output() closeModal = new EventEmitter<void>();
  @ContentChild('modalContent') modalContent: any;

  constructor() { }

  ngOnInit(): void {
  }
  ngAfterContentInit() {
    if (this.modalContent) {
      // You can do something with the modal content if needed
    }
  }
  close(): void {
    this.isVisible = false;
    this.closeModal.emit();
  }
}
