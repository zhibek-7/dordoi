import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProjectsService } from '../services/projects.service';
import { RequestDataReloadService } from 'src/app/glossaries/services/requestDataReload.service';
import { Project } from '../models/Project';
//import { FormBuilder, FormGroup, FormArray, FormControl } from '@angular/forms';
//import {Langs} from './langs';
//import { ListFilterPipe } from 'angular2-multiselect-dropdown';
//import { FilterPipe }from './filter.pipe';
@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css'],
})
export class CreateProjectComponent implements OnInit{
  args='ascending';
  reverse=false;
  searchText='';
  form: FormGroup;
  title = 'Создание проекта Crowdin';

  forms:Array<any>=[
    {labelName:'Название проекта:',placeHolder:'Обычно название вашего веб-сайта или приложения', ivalid_feedback:'Это поле необходимо заполнить',id:'exampleInputName', describedby:'emailHelp'},
    {labelName:'Адрес проекта:',placeHolder:'https://crowdin.com/project/<identifier>', ivalid_feedback:'Это поле необходимо заполнить',id:'exampleInputAddress', describedby:'addressHelp'},
   ];


    options:Array<any>=[
        {labelName:'Public project',id:'gridRadios1', small:'Visible to anyone. You can restrict access to languages after the project is created.',inputName:'gridRadios', value:'checked'},
        {labelName:'Private project',id:'gridRadios2', small:'Visible only to the invited project members',inputName:'gridRadios', value:'option2'},];
  

///////////////////////////////////////////////////

    dropdownList=[];
    selectedItems=[];
    dropdownSettings={};   
        ngOnInit() {
        this.dropdownList=[
          {itemName:'Ido', checked: false,id:'gridIdo'},
          {itemName:'Аварский', checked: false,id:'gridAvarski'},
          {itemName:'Азербайджанский',checked: false, id:'gridAzerbaidjan'},
          {itemName:'Авестийский', checked: false,id:'gridAvestiski'},
          {itemName:'Аймара',checked: false, id:'gridAimara'},
          {itemName:'Акан',checked: false, id:'gridAkan'},
          {itemName:'Албанский',checked: false, id:'gridAlbanski'},
          {itemName:'Амхарский',checked: false, id:'gridAmharskii'},
          {itemName:'Английский',checked: false, id:'gridEnglisch'},
          {itemName:'Английский (вверх ногами)',checked: false, id:'gridEnglVverhNogami'},
          {itemName:'Английский, Аравия',checked: false, id:'gridEnglAraviya'},
          {itemName:'Ангийский, Белиз',checked: false, id:'gridEnglBeliz'},
          {itemName:'Русский', checked: false, id:'gridRussia'},
        ];
      }

AddSelected(event,lang){
  var target = event.target || event.srcElement || event.currentTarget;
  var idAttr = target.attributes.id;
  var itemNameAttr = target.attributes.name;
  var selectedAttr = target.attributes.checked;
  var value = idAttr.nodeValue;
  lang.checked = !lang.checked;
  if(lang.checked==true){
    this.selectedItems.push({'itemName':itemNameAttr.nodeValue,'selected':lang.checked,'id':idAttr.nodeValue});
  }else{
    const index = this.selectedItems.findIndex(list => list.id == lang.id);//Find the index of stored id
    this.selectedItems.splice(index, 1); 
  } 
}

AllSelected(){
this.dropdownList.forEach(element => {
  element.checked = !element.checked;
  if(element.checked==true){
    this.selectedItems.push({'itemName':element.itemName,'selected':element.checked,'id':element.id});
  }else{
    const index = this.selectedItems.findIndex(list => list.id == element.id);//Find the index of stored id
    this.selectedItems.splice(index, 1); 
  } 
});}

FiterByName(){
  this.dropdownList.forEach(element => {
    element.checked = !element.checked;
    if(element.checked==true){
      this.selectedItems.push({'itemName':element.itemName,'selected':element.checked,'id':element.id});
    }else{
      const index = this.selectedItems.findIndex(list => list.id == element.id);//Find the index of stored id
      this.selectedItems.splice(index, 1); 
    } 
  });}


  private _visible: boolean = false;

  get visible() { return this._visible }

  @Input()
  set visible(value: boolean) {
    this._visible = value;
    this.visibleChange.emit(this._visible);
  }

  @Output()
  visibleChange = new EventEmitter<boolean>();

  constructor(private projectService: ProjectsService) { }

  
  
  //myForm = new FormGroup({
  //  name: new FormControl(null),
  //  url: new FormControl(null),
  //  language: new FormControl(null)
  //});

  addProject(newProject: Project) {
    //this.newProject.name = "ccccc";
    //this.newProject.ableToDownload = true;
    //this.newProject.ableToLeftErrors = true;
    //this.newProject.dateOfCreation = "12.12.2018";
    //this.newProject.defaultString = "12.12.2019";
    //this.newProject.description = "wdwdwdwd";
    //this.newProject.ID_SourceLocale = 12;
    //this.newProject.lastActivity = "qqq";
    //this.newProject.logo = "rr";
    //this.newProject.notifyConfirm = true;
    //this.newProject.notifyFinish = false;
    //this.newProject.notifyNew = true;
    //this.newProject.visibility = true;
    //this.newProject.id = Math.floor(Math.random() * 10000) + 1;
    //this.newProject.url = "rrrr";
    this.projectService.addProject(newProject);
  }

  editProject(Id: number) {
    this.projectService.updateProject(Id);
  }

  deleteProject(Id: number) {
    this.projectService.deleteProject(Id);
  }

  show() {
    this.visible = true;
  }

  hide() {
    this.visible = false;
  }
}
