import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PortfolioVM } from '../shared/models/PortfolioVM';
import { ShowPortfolioService } from '../services/show-portfolio.service';

@Component({
  selector: 'app-show-portfolio',
  templateUrl: './show-portfolio.component.html'
})
export class ShowPortfolioComponent {
  public portfolioVM: PortfolioVM;
  public errorFlag: Boolean = false;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private showPortfolioService: ShowPortfolioService) {
    showPortfolioService.getPortfolio().subscribe(result => {
      this.portfolioVM = result;
    }, error => this.errorFlag = true);
  }
}
