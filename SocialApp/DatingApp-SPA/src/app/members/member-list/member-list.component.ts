import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_services/alertify.service';
import { UserService } from '../../_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { Gender, mapStringToGender, mapGenderToString } from 'src/app/_models/gender.enum';
import { mapOrderToString, Order } from 'src/app/_models/order.enum';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})

export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{value: mapGenderToString(Gender.Male), display: 'Males'}, {value: mapGenderToString(Gender.Female), display: 'Females'}];
  userParams: UserParams = new UserParams();
  pagination: Pagination;

  constructor(private userService: UserService, private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });

    this.userParams.gender = this.user.gender === mapGenderToString(Gender.Male) ?
      mapGenderToString(Gender.Female) : mapGenderToString(Gender.Male);
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = mapOrderToString(Order.LastActive);
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  resetFilters() {
    this.userParams.gender = this.user.gender === mapGenderToString(Gender.Male) ?
      mapGenderToString(Gender.Female) : mapGenderToString(Gender.Male);
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;

    this.loadUsers();
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
      .subscribe((res: PaginatedResult<User[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error(error);
      });
  }
}
