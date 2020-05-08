import { Component, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import { ActivatedRoute } from '@angular/router';
import { FundService } from '../../services/fund.service';
@Component({
  selector: 'app-current-fund-setting',
  templateUrl: './current-fund-setting.component.html',
  styleUrls: ['./current-fund-setting.component.css']
})
export class CurrentFundSettingComponent implements OnInit {

  fundId: Guid;
  fundName: string;
  constructor(private route: ActivatedRoute,
    private fundService: FundService) { }

  ngOnInit() {
    this.fundId = this.fundService.currentFundId;
    this.fundName = this.fundService.currentFundName;



    console.log(".CurrentFundSettingsComponent==" + this.route.snapshot.params["fundId"]);
    if (this.route.snapshot.params["fundId"] != null) {
     
      this.fundService.currentFundId = this.route.snapshot.params["fundId"];
    }

    console.log("CurrentFundSettingsComponent ngOnInit() fundId: ", this.fundId);
    console.log("CurrentFundtSettingsComponent ngOnInit() fundName: ", this.fundName);

  }

}
