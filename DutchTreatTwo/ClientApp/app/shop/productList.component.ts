import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Product } from "../shared/product";

@Component({
   selector: "product-list",
   templateUrl: "productList.component.html",
   styleUrls: []
})
export class ProductList implements OnInit {
   
   constructor(private dataService: DataService) {
   }
   
   public products: Product[];

   ngOnInit(): void {
      this.dataService.loadProducts()
         .subscribe(success => {
            if (success) {
               this.products = this.dataService.products;
            }
         });
   }

}