import { Component, OnInit } from '@angular/core';
import { fund } from '../../models/database-entities/fund.type';
import { FundService } from '../../services/fund.service';

@Component({
  selector: 'app-create-fund',
  templateUrl: './create-fund.component.html',
  styleUrls: ['./create-fund.component.css']
})
export class CreateFundComponent implements OnInit {

  fund: fund;
  loaded: boolean = false;

  constructor(private fundService: FundService) { }

  ngOnInit() {
    this.fund = new fund(null, null);
    this.fund.date_time_added = null;
    //this.fund.ID_User=
    this.loaded = true;
  }

  submit(editedFund: fund) {
    this.fundService.create(editedFund).subscribe(
      fundId => {
        this.fundService.currentFundName = editedFund.name_text;
        this.fundService.currentFundDescription = editedFund.description;

      },
      error => console.error(error)
    );
  }


}
