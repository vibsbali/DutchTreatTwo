class StoreCustomer {

  constructor(private firstName:string, private lastName:string) {
  }

  public visits: number = 0;
  private ourName:string;

  public showName() {
    alert(this.firstName + " " + this.lastName);
  }

  //set name(val:string) {
  //  this.ourName = val;
  //}

  //get name():string {
  //  return this.ourName;
  //}
}