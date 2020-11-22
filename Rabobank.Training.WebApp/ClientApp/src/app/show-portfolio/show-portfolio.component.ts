import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-show-portfolio',
  templateUrl: './show-portfolio.component.html'
})
export class ShowPortfolioComponent {
  public portfolioVM: PortfolioVM;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<PortfolioVM>(baseUrl + 'api/Portfolio').subscribe(result => {
      this.portfolioVM = result;
      console.log(this.portfolioVM);
    }, error => console.error(error));
  }
}

interface MandateVM {
  value: string;
  name: string;
  allocation: number;
}

interface PositionVM {
  code: string;
  name: string;
  value: number;
  mandates: MandateVM[];
}

interface PortfolioVM {
  positions: PositionVM[];
}
