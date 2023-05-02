import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Subscription } from 'rxjs';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  Query,
} from '@angular/core';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { ItemService } from '../../services/item.service';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css'],
  //changeDetection: ChangeDetectionStrategy.OnPush,
})
export class sidenavComponent implements OnInit, OnDestroy {
  public checkList: CheckeBoxes[] = [];
  public brandsQuery: string[] = [];
  public storagesQuery: string[] = [];
  public shopsQuery: string[] = [];
  public conditionsQuery: string[] = [];
  public typesQuery: string[] = [];
  public carriersQuery: string[] = [];
  public excludesQuery: string[] = [];
  public sortByNow: string | null;
  public dealQuery: string | null;
  public maxPrice: string;
  public minPrice: string;
  private queryParams: URLSearchParams = new URLSearchParams();

  public radioButton: any = null;
  public keyword: string;

  private subscription: Subscription;

  public sortBy: Array<any> = [
    ['price-low-high', 'Low-High'],
    ['price-high-low', 'High-Low'],
    ['saving-high-low', 'Saving'],
    ['saving-percent-high-low', 'Saving Percent'],
  ];
  public brandsList: string[] = [
    'samsung',
    'iphone',
    'lg',
    'huawei',
    'alcatel',
    'motorola',
    'xiaomi',
    'htc',
  ];
  public storagesList: string[] = ['32', '64', '128', '256', '512'];
  public typesList: Array<any> = [
    ['accessory', '1'],
    ['headphone', '2'],
    ['memory', '3'],
    ['microphone', '4'],
    ['phone', '5'],
    ['speaker', '6'],
    ['streaming', '7'],
    ['tV', '8']
  ];
  public carriersList: string[] = [
    'unlocked',
    'boost',
    'cricket',
    'verizon',
    'at&t',
    't-mobile',
    'consumer cellular',
    'us cellular',
  ];
  public shopsList: Array<any> = [
    ['Amazon', '1'],
    ['eBay', '2'],
    ['TheStore', '3'],
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ItemService,
    private http: HttpClient,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    this.subscription = this.route.queryParams.subscribe((query) => {
      this.queryParams = new URLSearchParams(query);

      this.brandsQuery =
        this.queryParams.get('brands') != null
          ? this.queryParams.get('brands')?.split('%2C')!
          : [];
      this.storagesQuery =
        this.queryParams.get('storages') != null
          ? this.queryParams.get('storages')?.split('%2C')!
          : [];
      this.shopsQuery =
        this.queryParams.get('shops') != null
          ? this.queryParams.get('shops')?.split('%2C')!
          : [];
      this.conditionsQuery =
        this.queryParams.get('condition') != null
          ? this.queryParams.get('condition')?.split('%2C')!
          : [];
      this.typesQuery =
        this.queryParams.get('types') != null
          ? this.queryParams.get('types')?.split('%2C')!
          : [];
      this.carriersQuery =
        this.queryParams.get('carriers') != null
          ? this.queryParams.get('carriers')?.split('%2C')!
          : [];
      this.excludesQuery =
        this.queryParams.get('excludes') != null
          ? this.queryParams.get('excludes')?.split('%2C')!
          : [];
      this.maxPrice =
        this.queryParams.get('max') != null ? this.queryParams.get('max')! : '';
      this.minPrice =
        this.queryParams.get('min') != null ? this.queryParams.get('min')! : '';
      this.dealQuery =
        this.queryParams.get('offer') != null
          ? this.queryParams.get('offer')
          : null;
      this.service.serch$.next(
        this.queryParams.get('search') != null
          ? this.queryParams.get('search')!
          : ''
      );
      this.sortByNow =
        this.queryParams.get('sort_by');

      //this.brandsExpande = this.brandsQuery.length > 0;
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    console.log('destroy', 'sidenav');
  }

  //#region Cleaning
  cleanShops() {
    this.shopsQuery = [];
    this.DeleteElement(this.excludesQuery, 'shops');
    this.SettingsNavigation();
  }

  cleanOrder() {
    this.queryParams.delete('sort_by');
    console.log(this.queryParams.toString())
    this.SettingsNavigation();
  }
  cleanDeals(): void {
    this.dealQuery = null;
    this.router.navigate([]);
  }

  cleanBrands() {
    this.brandsQuery = [];
    this.DeleteElement(this.excludesQuery, 'brands');
    this.SettingsNavigation();
  }

  cleanTypes() {
    this.typesQuery = [];
    this.SettingsNavigation();
  }

  cleanConditions() {
    this.conditionsQuery = [];
    this.SettingsNavigation();
  }

  cleanCarriers() {
    this.carriersQuery = [];
    this.DeleteElement(this.excludesQuery, 'carriers');
    this.SettingsNavigation();
  }

  cleanStorages() {
    this.storagesQuery = [];
    this.DeleteElement(this.excludesQuery, 'storages');
    this.SettingsNavigation();
  }

  cleanPrices() {
    this.queryParams.delete('min');
    this.queryParams.delete('max');
    this.SettingsNavigation();
  }

  CleanAll(): void {
    this.service.select$.next(0);
    this.router.navigate([]);
  }

  //#endregion
  Deal(e: any): void {
    this.router.navigate([], {
      queryParams: { offer: e.value },
    });
  }

  OrderByChange(e: any): void {
    this.queryParams.set('sort_by', e.value);
    this.SettingsNavigation();
  }

  DeleteElement(category: string[], data: string): void {
    const index = category.indexOf(data, 0);
    if (index > -1) {
      category.splice(index, 1);
    }
  }

  exclude(e: any): void {
    const name = e.source.name;
    const checked = e.checked;

    if (name === 'brands') {
      this.settingQueryParams('brands', checked, this.excludesQuery);
    }
    if (name === 'carriers') {
      this.settingQueryParams('carriers', checked, this.excludesQuery);
    }
    if (name === 'storages') {
      this.settingQueryParams('storages', checked, this.excludesQuery);
    }
    if (name === 'types') {
      this.settingQueryParams('types', checked, this.excludesQuery);
    }

    this.SettingsNavigation();
  }


  Reload() {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate(['./'], {
      relativeTo: this.route,
      queryParamsHandling: 'preserve',
    });
  }

  banKeyword(keyword: string) {

    var api = environment.baseApi + 'Items/BanKeyword'
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });


    this.http.post(api,{keyword}).subscribe(
      {
        next: (data) => {

          this.Reload();
          this.toastr.success(`${keyword} banned`);
          this.keyword = "";
        }
      }
    );
  }


  priceChange(e: any): void {
    //MAXIMUS
    if (e.max.value === undefined || e.max.value.length === 0) {
      this.queryParams.delete('max');
    } else {
      this.queryParams.set('max', e.max.value);
    }
    //MINIMUN
    if (e.min.value === undefined || e.min.value.length === 0) {
      this.queryParams.delete('min');
    } else {
      this.queryParams.set('min', e.min.value);
    }

    this.SettingsNavigation();
  }

  settingQueryParams(
    params: string,
    checked: boolean,
    category: string[]
  ): void {
    if (checked === true) {
      category.push(params);
    } else {
      this.DeleteElement(category, params);
    }
  }

  //#region Checkbox Change
  checkboxChange(e: any): void {
    //BRANDS
    if (e.id === 'samsung') {
      this.settingQueryParams('samsung', e.checked, this.brandsQuery);
    }
    if (e.id === 'iphone') {
      this.settingQueryParams('iphone', e.checked, this.brandsQuery);
    }

    if (e.id === 'lg') {
      this.settingQueryParams('lg', e.checked, this.brandsQuery);
    }

    if (e.id === 'xiaomi') {
      this.settingQueryParams('xiaomi', e.checked, this.brandsQuery);
    }

    if (e.id === 'huawei') {
      this.settingQueryParams('huawei', e.checked, this.brandsQuery);
    }

    if (e.id === 'alcatel') {
      this.settingQueryParams('alcatel', e.checked, this.brandsQuery);
    }

    if (e.id === 'htc') {
      this.settingQueryParams('htc', e.checked, this.brandsQuery);
    }
    if (e.id === 'motorola') {
      this.settingQueryParams('motorola', e.checked, this.brandsQuery);
    }

    //CONDITON
    if (e.id === 'new') {
      this.settingQueryParams('1', e.checked, this.conditionsQuery);
    }

    if (e.id === 'used') {
      this.settingQueryParams('2', e.checked, this.conditionsQuery);
    }

    //STORAGE

    if (e.id === '32') {
      this.settingQueryParams('32gb', e.checked, this.storagesQuery);
      this.settingQueryParams('32 gb', e.checked, this.storagesQuery);
    }

    if (e.id === '64') {
      this.settingQueryParams('64gb', e.checked, this.storagesQuery);
      this.settingQueryParams('64 gb', e.checked, this.storagesQuery);
    }

    if (e.id === '128') {
      this.settingQueryParams('128gb', e.checked, this.storagesQuery);
      this.settingQueryParams('128 gb', e.checked, this.storagesQuery);
    }

    if (e.id === '256') {
      this.settingQueryParams('256gb', e.checked, this.storagesQuery);
      this.settingQueryParams('256 gb', e.checked, this.storagesQuery);
    }

    if (e.id === '512') {
      this.settingQueryParams('512gb', e.checked, this.storagesQuery);
      this.settingQueryParams('512 gb', e.checked, this.storagesQuery);
    }

    /*    ['Accessory', '1'],
          ['Headphone', '2'],
          ['Memory', '3'],
          ['Microphone', '4'],
          ['Phone', '5'],
          ['Speaker', '6'],
          ['Streaming', '7'],
          ['TV', '8']*/

    //TYPE
    if (e.id === 'phone') {
      this.settingQueryParams('5', e.checked, this.typesQuery);
    }
    if (e.id === 'tv') {
      this.settingQueryParams('8', e.checked, this.typesQuery);
    }
    if (e.id === 'memory') {
      this.settingQueryParams('3', e.checked, this.typesQuery);
    }
    if (e.id === 'headphone') {
      this.settingQueryParams('2', e.checked, this.typesQuery);
    }
    if (e.id === 'speaker') {
      this.settingQueryParams('6', e.checked, this.typesQuery);
    }
    if (e.id === 'microphone') {
      this.settingQueryParams('4', e.checked, this.typesQuery);
    }
    if (e.id === 'streaming') {
      this.settingQueryParams('7', e.checked, this.typesQuery);
    }
    if (e.id === 'accesory') {
      this.settingQueryParams('1', e.checked, this.typesQuery);
    }

    //CARRIER

    if (e.id === 'boost') {
      this.settingQueryParams('boost', e.checked, this.carriersQuery);
    }
    if (e.id === 'cricket') {
      this.settingQueryParams('cricket', e.checked, this.carriersQuery);
    }
    if (e.id === 'verizon') {
      this.settingQueryParams('verizon', e.checked, this.carriersQuery);
    }
    if (e.id === 'at&t') {
      this.settingQueryParams('at%26t', e.checked, this.carriersQuery);
      this.settingQueryParams('att', e.checked, this.carriersQuery);
    }

    //'t-mobile','consumer cellular','us cellular'

    if (e.id === 't-mobile') {
      this.settingQueryParams('t-mobile', e.checked, this.carriersQuery);
      this.settingQueryParams('t%2Bmobile', e.checked, this.carriersQuery);
    }
    if (e.id === 'consumer cellular') {
      this.settingQueryParams(
        'consumer%2Bcellular',
        e.checked,
        this.carriersQuery
      );
    }
    if (e.id === 'us cellular') {
      this.settingQueryParams('us%2Bcellular', e.checked, this.carriersQuery);
    }

    if (e.id === 'unlocked') {
      this.settingQueryParams('unlock', e.checked, this.carriersQuery);
    }

    //SHOP
    if (e.id === 'Amazon') {
      this.settingQueryParams('1', e.checked, this.shopsQuery);
    }
    if (e.id === 'eBay') {
      this.settingQueryParams('2', e.checked, this.shopsQuery);
    }
    if (e.id === 'TheStore') {
      this.settingQueryParams('3', e.checked, this.shopsQuery);
    }

    this.SettingsNavigation();
  }
  //#endregion

  SettingsNavigation() {
    let queryJson;
    const listParams = [
      [this.brandsQuery, 'brands'],
      [this.storagesQuery, 'storages'],
      [this.conditionsQuery, 'condition'],
      [this.typesQuery, 'types'],
      [this.carriersQuery, 'carriers'],
      [this.excludesQuery, 'excludes'],
      [this.shopsQuery, 'shops']
    ];

    for (let i = 0; i < listParams.length; i++) {
      const category = listParams[i][0];
      const nameCategory = listParams[i][1].toString();

      if (category.length > 0) {
        this.queryParams.append(nameCategory, category.toString());
      } else {
        this.queryParams.delete(nameCategory);
      }
    }

    //QUERY JSON
    if (this.queryParams.toString() !== '') {
      queryJson = JSON.parse(
        '{"' +
        decodeURI(
          this.queryParams
            .toString()
            .replace(/&/g, '","')
            .replace(/=/g, '":"')
        ) +
        '"}'
      );
    } else {
      queryJson = {};
    }

    //NAVIGATION
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: queryJson,
    });
    //SCROLL TO UP
    document.querySelector('mat-sidenav-content')?.scroll(0, 0);
  }
}

class CheckeBoxes {
  name: string;
  checked: boolean;
}
