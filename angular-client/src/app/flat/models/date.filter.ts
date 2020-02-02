
export class DateFilter {

  dfrom: Date;
  dto: Date;

  public static getNew(): DateFilter {
    return new DateFilter(); 
  }

  constructor() {
    this.dfrom = new Date();
    this.dto = new Date();
  }
}

