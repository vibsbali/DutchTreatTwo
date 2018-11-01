import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Product } from "./product";

@Injectable()
export class DataService {
   //public products = [
   //   {
   //      title: "First Product",
   //      price: 19.99
   //   },
   //   {
   //      title: "Second Product",
   //      price: 9.99
   //   },
   //   {
   //      title: "Third Product",
   //      price: 11.99
   //   }
   //];

   constructor(private http: HttpClient) {

   }

   public products: Product[] = [];

   loadProducts(): Observable<boolean> {
      return this.http.get("/api/products")
         .pipe(
            map((data: any[]) => {
            this.products = data;
               return true;
            })
         );
   }
}