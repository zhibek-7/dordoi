import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';

import { TimeZoneService } from 'src/app/services/time-zone.service';

import { TimeZone } from 'src/app/models/database-entities/timeZone.type';

@Component({
  selector: 'app-set-time-zone',
  templateUrl: './set-time-zone.component.html',
  styleUrls: ['./set-time-zone.component.css']
})
export class SetTimeZoneComponent implements OnInit {

  constructor(private timeZoneService: TimeZoneService) { }

  @Input() availableTimeZone: number;
  @Output() selectedTimeZoneChanged = new EventEmitter<TimeZone[]>();

  timeZones: TimeZone[];
  selectedTimeZone: TimeZone;

  ngOnInit() {
    this.timeZoneService.getAll()
      .subscribe(timeZones => {
        this.timeZones = timeZones;
        },
        error => console.error(error));
  }

  raiseSelectionChanged(event: any) {
    this.selectedTimeZoneChanged.emit(event);
  }

}
