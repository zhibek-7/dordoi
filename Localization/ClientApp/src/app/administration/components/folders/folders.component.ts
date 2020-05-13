import { Component, Injectable, OnInit } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { CollectionViewer, SelectionChange } from '@angular/cdk/collections';
import { BehaviorSubject, Observable, merge } from 'rxjs';
import { map } from 'rxjs/operators';
//import { ApiService } from '../../../services/api.service';
//import { Resource } from '../../../models/Resource';
import { HttpClient } from '@angular/common/http';

function getDataMap(parentName) {
  return new Map([
    [parentName, ['Apple', 'Orange', 'Banana']]
    // ['Fruits', ['Apple', 'Orange', 'Banana']],
    // ['Vegetables', ['Tomato', 'Potato', 'Onion']],
    // ['Apple', ['Fuji', 'Macintosh']],
    // ['Onion', ['Yellow', 'White', 'Purple']]
  ]);
}

export class DynamicDatabase {
  rootLevelNodes = ['assest'];
  dataMap;
  // = getDataMap(this.rootLevelNodes[0]);


  /** Initial data from database */
  initialData(): DynamicFlatNode[] {
    console.log(888);
    return this.rootLevelNodes.map(name => new DynamicFlatNode(name, 0, true));
  }

  getChildren(node: string): string[] | undefined {
    return this.dataMap.get(node);
  }

  isExpandable(node: string): boolean {
    return this.dataMap.has(node);
  }
}

@Injectable()
export class DynamicDataSource {
  dataChange: BehaviorSubject<DynamicFlatNode[]> = new BehaviorSubject<
    DynamicFlatNode[]
  >([]);

  get data(): DynamicFlatNode[] {
    return this.dataChange.value;
  }
  set data(value: DynamicFlatNode[]) {
    this.treeControl.dataNodes = value;
    this.dataChange.next(value);
  }

  constructor(
    private treeControl: FlatTreeControl<DynamicFlatNode>,
    private database: DynamicDatabase
  ) {}

  connect(collectionViewer: CollectionViewer): Observable<DynamicFlatNode[]> {
    this.treeControl.expansionModel.onChange!.subscribe(change => {
      if (
        (change as SelectionChange<DynamicFlatNode>).added ||
        (change as SelectionChange<DynamicFlatNode>).removed
      ) {
        this.handleTreeControl(change as SelectionChange<DynamicFlatNode>);
      }
    });

    return merge(collectionViewer.viewChange, this.dataChange).pipe(
      map(() => this.data)
    );
  }

  /** Handle expand/collapse behaviors */
  handleTreeControl(change: SelectionChange<DynamicFlatNode>) {
    if (change.added) {
      change.added.forEach(node => this.toggleNode(node, true));
    }
    if (change.removed) {
      change.removed.reverse().forEach(node => this.toggleNode(node, false));
    }
  }

  /**
   * Toggle the node, remove from display list
   */
  toggleNode(node: DynamicFlatNode, expand: boolean) {
    const children = this.database.getChildren(node.item);
    const index = this.data.indexOf(node);
    if (!children || index < 0) {
      // If no children, or cannot find the node, no op
      return;
    }

    node.isLoading = true;

    setTimeout(() => {
      if (expand) {
        const nodes = children.map(
          name =>
            new DynamicFlatNode(
              name,
              node.level + 1,
              this.database.isExpandable(name)
            )
        );
        this.data.splice(index + 1, 0, ...nodes);
      } else {
        this.data.splice(index + 1, children.length);
      }

      // notify the change
      this.dataChange.next(this.data);
      node.isLoading = false;
    }, 1000);
  }
}

@Component({
  selector: 'app-folders',
  templateUrl: './folders.component.html',
  styleUrls: ['./folders.component.css'],
  providers: [DynamicDatabase]
})
export class FoldersComponent {
  private currentdir = '/';
  //private resoursesData: Resource[];
  folder;

  constructor(public database: DynamicDatabase,
    //public apiClient: ApiService
  ) {
    this.database.dataMap =
    //apiClient
    //  .getResource(
    //    this.currentdir.substring(0, this.currentdir.lastIndexOf('/')),
    //    // tslint:disable-next-line:max-line-length
    //    'eyJhbGciOiJIUzI1NiIsInppcCI6IkdaSVAifQ.H4sIAAAAAAAAAKtWyiwuVrJSSs5PSS3KyczLVtJRykwsUbIyNDWxMDY2NTc20VFKrSiACJgYGRmCBXITM3OAuhJTcjPzjBxAPL3k_FylWgCHcYbMTwAAAA.zUIBZlYhL0JrFVfkYBQ1KYZ66JiZppOg5pLQcDzmbrk',
    //    'fileasc'
    //  )
    //  .subscribe({
    //    next: (data: Resource[]) => {
    //      this.addChildNode(data);
    //    },
    //    error: err => console.log(err),
    //    complete: () => {
    //      console.log('complite');
    //    }
    //  });
    this.treeControl = new FlatTreeControl<DynamicFlatNode>(
      this.getLevel,
      this.isExpandable
    );
    this.dataSource = new DynamicDataSource(this.treeControl, database);

    console.log(new DynamicDataSource(this.treeControl, database), 'dataSorce')

    this.dataSource.data = database.initialData();
  }

  treeControl: FlatTreeControl<DynamicFlatNode>;

  dataSource: DynamicDataSource;

  getLevel = (node: DynamicFlatNode) => node.level;

  isExpandable = (node: DynamicFlatNode) => node.expandable;

  hasChild = (_: number, _nodeData: DynamicFlatNode) => _nodeData.expandable;

  addChildNode(data) {
    console.log(666, 777, data);
    // this.folder = path;
    // if (type !== 'folder') {
    //   return;
    // }
    // path === '/'
    //   ? (this.currentdir = path)
    //   : (this.currentdir = this.currentdir + path + '/');
    // this.getResource();
    // this.dataSource.data = this.database.initialData();
  }

  changeChildNode() {
    //console.log(this.resoursesData, 777);
  }
}

export class DynamicFlatNode {
  constructor(
    public item: string,
    public level: number = 1,
    public expandable: boolean = false,
    public isLoading: boolean = false
  ) {}
}

export class FileNode {
  children: FileNode[];
  filename: string;
  type: any;
}

const foldersStructure = {
  assets: {
    // images: {},
    // testPdf: 'pdf',
    // testPdf1: 'pdf',
    // testPdf2: 'pdf',
    // testPdf3: 'pdf',
  }
};

const TREE_DATA = JSON.stringify(foldersStructure);
