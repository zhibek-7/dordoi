import { Component, ViewEncapsulation, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";

import { Image as ImageModel } from "src/app/models/database-entities/image.type";

import { ImagesService } from "src/app/services/images.service";

declare var $: any;

export interface ImageEditingModalComponentInputData {
  image: ImageModel;
  clickEventArgs: any;
}

@Component({
  selector: "app-image-editing-modal",
  templateUrl: "./image-editing-modal.component.html",
  styleUrls: ["./image-editing-modal.component.css"],
  encapsulation: ViewEncapsulation.None
})
export class ImageEditingModalComponent implements OnInit {

  currentImg: ImageModel;

  mouse = {
    x: 0,
    y: 0,
    startX: 0,
    startY: 0
  };

  mouseSqr = {
    x: 0,
    y: 0,
    startX: 0,
    startY: 0
  };

  constructor(
    public dialogRef: MatDialogRef<ImageEditingModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ImageEditingModalComponentInputData,
    private imagesService: ImagesService,
  ) {
    this.currentImg = this.data.image;
  }

  ngOnInit(): void {
    const canvas = $('#imgCanvas')[0];
    const img = new Image();
    const ctx = canvas.getContext('2d');
    const canvasCopy = document.createElement('canvas');
    const copyctx = canvasCopy.getContext('2d');
    img.src = this.data.clickEventArgs.target.src;
    img.onload = function () {
      let ratio = 1;
      const maxWidth = 900;
      const maxHeight = 600;

      if (img.width > maxWidth) {
        ratio = maxWidth / img.width;
      } else if (img.height > maxHeight) {
        ratio = maxHeight / img.height;
      }

      canvasCopy.width = img.width;
      canvasCopy.height = img.height;
      copyctx.drawImage(img, 0, 0);

      canvas.width = img.width * ratio;
      canvas.height = img.height * ratio;
      ctx.drawImage(
        canvasCopy,
        0,
        0,
        canvasCopy.width,
        canvasCopy.height,
        0,
        0,
        canvas.width,
        canvas.height
      );
    };
  }

  close(): void {
    this.mouse = {
      x: 0,
      y: 0,
      startX: 0,
      startY: 0
    };
    this.mouseSqr = {
      x: 0,
      y: 0,
      startX: 0,
      startY: 0
    };
    $('.resizable').remove();
    $('.resizable-square').remove();
    $('.txt-input').remove();
    this.dialogRef.close();
  }

  selectArea(canvas) {
    // $('.resizable').remove();
    const that = this;
    const modalBody = $('#modal-body')[0];
    function setMousePosition(e) {
      const ev = e || window.event;
      mouseSqr.x = ev.offsetX;
      mouseSqr.y = ev.offsetY;
    }

    let element = null;
    let resizers = null;
    let resizer = null;

    const mouseSqr = {
      x: 0,
      y: 0,
      startX: 0,
      startY: 0
    };

    function makeResizableDiv(element) {
      const resizers = document.querySelectorAll('.resizer');
      const minimum_size = 20;
      let original_width = 0;
      let original_height = 0;
      let original_x = 0;
      let original_y = 0;
      let original_mouse_x = 0;
      let original_mouse_y = 0;

      for (let i = 0; i < resizers.length; i++) {
        const currentResizer = resizers[i];
        currentResizer.addEventListener('mousedown', function (e: MouseEvent) {
          e.preventDefault();
          original_width = parseFloat(
            getComputedStyle(element, null)
              .getPropertyValue('width')
              .replace('px', '')
          );
          original_height = parseFloat(
            getComputedStyle(element, null)
              .getPropertyValue('height')
              .replace('px', '')
          );
          original_x = element.getBoundingClientRect().left;
          original_y = element.getBoundingClientRect().top;
          original_mouse_x = e.pageX;
          original_mouse_y = e.pageY;

          function resize(e: MouseEvent) {
            if (currentResizer.classList.contains('bottom-right')) {
              // mouse.x = e.pageX;
              // mouse.y = e.pageY;
              // element.style.width = mouse.x - element.getBoundingClientRect().left + 'px';
              const width = original_width + (e.pageX - original_mouse_x);
              const height = original_height + (e.pageY - original_mouse_y);
              if (width > minimum_size) {
                element.style.width = width + 'px';
              }
              if (height > minimum_size) {
                element.style.height = height + 'px';
              }
            } else if (currentResizer.classList.contains('bottom-left')) {
              const height = original_height + (e.pageY - original_mouse_y);
              const width = original_width - (e.pageX - original_mouse_x);
              if (height > minimum_size) {
                element.style.height = height + 'px';
              }
              if (width > minimum_size) {
                element.style.width = width + 'px';
                element.style.left =
                  mouseSqr.startX + (e.pageX - original_mouse_x) + 'px';
              }
            } else if (currentResizer.classList.contains('top-right')) {
              const width = original_width + (e.pageX - original_mouse_x);
              const height = original_height - (e.pageY - original_mouse_y);
              if (width > minimum_size) {
                element.style.width = width + 'px';
              }
              if (height > minimum_size) {
                element.style.height = height + 'px';
                element.style.top =
                  mouseSqr.startY + (e.pageY - original_mouse_y) + 'px';
              }
            } else {
              const width = original_width - (e.pageX - original_mouse_x);
              const height = original_height - (e.pageY - original_mouse_y);
              if (width > minimum_size) {
                element.style.width = width + 'px';
                element.style.left =
                  mouseSqr.startX + (e.pageX - original_mouse_x) + 'px';
              }
              if (height > minimum_size) {
                element.style.height = height + 'px';
                element.style.top =
                  mouseSqr.startY + (e.pageY - original_mouse_y) + 'px';
              }
            }
          }

          function stopResize() {
            window.removeEventListener('mousemove', resize);
            const newElement = $('.resizable-square')[0];
            if (newElement) {
              mouseSqr.x = newElement.offsetWidth + newElement.offsetLeft;
              mouseSqr.y = newElement.offsetTop + newElement.offsetHeight;
              mouseSqr.startX = newElement.offsetLeft;
              mouseSqr.startY = newElement.offsetTop;
            }
          }

          window.addEventListener('mousemove', resize);
          window.addEventListener('mouseup', stopResize);
        });
      }
    }

    canvas.onmousemove = function (e) {
      setMousePosition(e);
      if (element !== null) {
        element.style.width = Math.abs(mouseSqr.x - mouseSqr.startX) + 'px';
        element.style.height = Math.abs(mouseSqr.y - mouseSqr.startY) + 'px';
        element.style.left =
          mouseSqr.x - mouseSqr.startX < 0 ? mouseSqr.x + 'px' : mouseSqr.startX + 'px';
        element.style.top =
          mouseSqr.y - mouseSqr.startY < 0 ? mouseSqr.y + 'px' : mouseSqr.startY + 'px';
      }
    };

    function createReizers(el) {
      resizers = document.createElement('div');
      resizers.className = 'resizers';
      resizers.style.width = '100%';
      resizers.style.height = '100%';
      resizers.style.position = 'absolute';
      resizers.style['box-sizing'] = 'border-box';
      element.appendChild(resizers);

      resizer = document.createElement('div');
      resizer.className = 'resizer top-left';
      resizer.style.left = '-5px';
      resizer.style.top = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '1px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nwse-resize';
      resizers.appendChild(resizer);

      resizer = document.createElement('div');
      resizer.className = 'resizer top-right';
      resizer.style.right = '-5px';
      resizer.style.top = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '3px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nesw-resize';
      resizers.appendChild(resizer);

      resizer = document.createElement('div');
      resizer.className = 'resizer bottom-right';
      resizer.style.right = '-5px';
      resizer.style.bottom = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '3px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nwse-resize';
      resizers.appendChild(resizer);

      resizer = document.createElement('div');
      resizer.className = 'resizer bottom-left';
      resizer.style.left = '-5px';
      resizer.style.bottom = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '3px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nesw-resize';
      resizers.appendChild(resizer);
    }

    canvas.onclick = function (e) {
      if (element !== null) {
        resizers = null;
        resizer = null;
        canvas.style.cursor = 'default';
        canvas.onmousemove = function () { };
        that.mouse = mouseSqr;
        console.log('finsihed.', mouseSqr);

        createReizers(element);
        makeResizableDiv(element);
        element = null;
      } else {
        console.log('begun.');
        mouseSqr.startX = mouseSqr.x;
        mouseSqr.startY = mouseSqr.y;
        element = document.createElement('div');
        element.className = 'resizable-square';
        element.style.border = '2px solid #FF0000';
        element.style.position = 'absolute';
        element.style.left = mouseSqr.x + 'px';
        element.style.top = mouseSqr.y + 'px';

        modalBody.appendChild(element);
        canvas.style.cursor = 'crosshair';
      }
    };
  }

  saveSqr() {
    const squares = $('.resizable-square');
    const canvas = $('#imgCanvas')[0];
    const ctx = canvas.getContext('2d');
    for (let i = 0; i < squares.length; i++) {
      ctx.strokeStyle = 'red';
      ctx.strokeRect(squares[i].offsetLeft, squares[i].offsetTop, squares[i].clientWidth, squares[i].clientHeight);
      squares[i].remove();
    }
  }

  cutImg() {
    const that = this;
    const canvas = $('#imgCanvas')[0];
    const ctx = canvas.getContext('2d');
    const widthX = this.mouse.x - this.mouse.startX;
    const heihgtY = this.mouse.y - this.mouse.startY;
    const imgData = ctx.getImageData(
      this.mouse.startX,
      this.mouse.startY,
      widthX,
      heihgtY
    );
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.putImageData(imgData, 0, 0);
    $('.resizable').remove();
    canvas.onclick = null;
  }

  initDraw(canvas) {
    $('.resizable').remove();
    const that = this;
    const modalBody = $('#modal-body')[0];
    function setMousePosition(e) {
      const ev = e || window.event;
      mouse.x = ev.offsetX;
      mouse.y = ev.offsetY;
    }

    let element = null;
    let resizers = null;
    let resizer = null;

    const mouse = {
      x: 0,
      y: 0,
      startX: 0,
      startY: 0
    };

    function makeResizableDiv(element) {
      const resizers = document.querySelectorAll('.resizer');
      const minimum_size = 20;
      let original_width = 0;
      let original_height = 0;
      let original_x = 0;
      let original_y = 0;
      let original_mouse_x = 0;
      let original_mouse_y = 0;

      for (let i = 0; i < resizers.length; i++) {
        const currentResizer = resizers[i];
        currentResizer.addEventListener('mousedown', function (e: MouseEvent) {
          e.preventDefault();
          original_width = parseFloat(
            getComputedStyle(element, null)
              .getPropertyValue('width')
              .replace('px', '')
          );
          original_height = parseFloat(
            getComputedStyle(element, null)
              .getPropertyValue('height')
              .replace('px', '')
          );
          original_x = element.getBoundingClientRect().left;
          original_y = element.getBoundingClientRect().top;
          original_mouse_x = e.pageX;
          original_mouse_y = e.pageY;

          function resize(e: MouseEvent) {
            if (currentResizer.classList.contains('bottom-right')) {
              // mouse.x = e.pageX;
              // mouse.y = e.pageY;
              // element.style.width = mouse.x - element.getBoundingClientRect().left + 'px';
              const width = original_width + (e.pageX - original_mouse_x);
              const height = original_height + (e.pageY - original_mouse_y);
              if (width > minimum_size) {
                element.style.width = width + 'px';
              }
              if (height > minimum_size) {
                element.style.height = height + 'px';
              }
            } else if (currentResizer.classList.contains('bottom-left')) {
              const height = original_height + (e.pageY - original_mouse_y);
              const width = original_width - (e.pageX - original_mouse_x);
              if (height > minimum_size) {
                element.style.height = height + 'px';
              }
              if (width > minimum_size) {
                element.style.width = width + 'px';
                element.style.left =
                  mouse.startX + (e.pageX - original_mouse_x) + 'px';
              }
            } else if (currentResizer.classList.contains('top-right')) {
              const width = original_width + (e.pageX - original_mouse_x);
              const height = original_height - (e.pageY - original_mouse_y);
              if (width > minimum_size) {
                element.style.width = width + 'px';
              }
              if (height > minimum_size) {
                element.style.height = height + 'px';
                element.style.top =
                  mouse.startY + (e.pageY - original_mouse_y) + 'px';
              }
            } else {
              const width = original_width - (e.pageX - original_mouse_x);
              const height = original_height - (e.pageY - original_mouse_y);
              if (width > minimum_size) {
                element.style.width = width + 'px';
                element.style.left =
                  mouse.startX + (e.pageX - original_mouse_x) + 'px';
              }
              if (height > minimum_size) {
                element.style.height = height + 'px';
                element.style.top =
                  mouse.startY + (e.pageY - original_mouse_y) + 'px';
              }
            }
          }

          function stopResize() {
            window.removeEventListener('mousemove', resize);
            const newElement = $('.resizable')[0];
            if (newElement) {
              mouse.x = newElement.offsetWidth + newElement.offsetLeft;
              mouse.y = newElement.offsetTop + newElement.offsetHeight;
              mouse.startX = newElement.offsetLeft;
              mouse.startY = newElement.offsetTop;
            }
          }

          window.addEventListener('mousemove', resize);
          window.addEventListener('mouseup', stopResize);
        });
      }
    }

    canvas.onmousemove = function (e) {
      setMousePosition(e);
      if (element !== null) {
        element.style.width = Math.abs(mouse.x - mouse.startX) + 'px';
        element.style.height = Math.abs(mouse.y - mouse.startY) + 'px';
        element.style.left =
          mouse.x - mouse.startX < 0 ? mouse.x + 'px' : mouse.startX + 'px';
        element.style.top =
          mouse.y - mouse.startY < 0 ? mouse.y + 'px' : mouse.startY + 'px';
      }
    };

    function createReizers(el) {
      resizers = document.createElement('div');
      resizers.className = 'resizers';
      resizers.style.width = '100%';
      resizers.style.height = '100%';
      resizers.style.position = 'absolute';
      resizers.style['box-sizing'] = 'border-box';
      element.appendChild(resizers);

      resizer = document.createElement('div');
      resizer.className = 'resizer top-left';
      resizer.style.left = '-5px';
      resizer.style.top = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '1px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nwse-resize';
      resizers.appendChild(resizer);

      resizer = document.createElement('div');
      resizer.className = 'resizer top-right';
      resizer.style.right = '-5px';
      resizer.style.top = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '3px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nesw-resize';
      resizers.appendChild(resizer);

      resizer = document.createElement('div');
      resizer.className = 'resizer bottom-right';
      resizer.style.right = '-5px';
      resizer.style.bottom = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '3px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nwse-resize';
      resizers.appendChild(resizer);

      resizer = document.createElement('div');
      resizer.className = 'resizer bottom-left';
      resizer.style.left = '-5px';
      resizer.style.bottom = '-5px';
      resizer.style.width = '10px';
      resizer.style.height = '10px';
      // resizer.style['border-radius'] = '50%';
      resizer.style.position = 'absolute';
      resizer.style.border = '3px solid #FF0000';
      resizer.style.background = '#FF0000';
      resizer.style.cursor = 'nesw-resize';
      resizers.appendChild(resizer);
    }

    canvas.onclick = function (e) {
      if (element !== null) {
        resizers = null;
        resizer = null;
        canvas.style.cursor = 'default';
        canvas.onmousemove = function () { };
        that.mouse = mouse;
        console.log('finsihed.', mouse);

        createReizers(element);
        makeResizableDiv(element);
        element = null;
      } else {
        console.log('begun.');
        mouse.startX = mouse.x;
        mouse.startY = mouse.y;
        element = document.createElement('div');
        element.className = 'resizable';
        element.style.border = '2px dashed #FF0000';
        element.style.position = 'absolute';
        element.style.left = mouse.x + 'px';
        element.style.top = mouse.y + 'px';

        modalBody.appendChild(element);
        canvas.style.cursor = 'crosshair';
      }
    };
  }

  getTxtInput(ev) {
    $('.resizable').remove();
    $('.txt-input').remove();
    const modalBody = $('#modal-body')[0];
    let cellText = null;
    modalBody.onclick = function (ev) {
      // console.log(cellText);
      if (cellText == null) {
        cellText = $('<textarea/>');
        cellText[0].className = 'txt-input';
        cellText[0].style.position = 'absolute';
        cellText[0].style.left = ev.offsetX + 'px';
        cellText[0].style.top = ev.offsetY + 'px';
        cellText[0].style.width = '200px';
        cellText[0].style.color = 'red';
        cellText[0].style['background-color'] = 'inherit';
        modalBody.append(cellText[0]);
      }
    };
  }

  saveTxt() {
    const inputsArr = $('.txt-input');
    const canvas = $('#imgCanvas')[0];
    const ctx = canvas.getContext('2d');
    for (let i = 0; i < inputsArr.length; i++) {
      const element = inputsArr[i];
      if (element.value !== '') {
        ctx.lineWidth = 1;
        ctx.fillStyle = 'black';
        ctx.lineStyle = '#ffff00';
        ctx.font = '18px sans-serif';
        const pos = $('.txt-input').position();

        const wrapText = function (
          context,
          text,
          marginLeft,
          marginTop,
          maxWidth,
          lineHeight
        ) {
          const entersArr = text.split(/\r?\n/);
          entersArr.forEach(string => {
            const words = string.split(' ');
            const countWords = words.length;
            let line = '';
            for (let n = 0; n < countWords; n++) {
              const testLine = line + words[n] + ' ';
              const testWidth = context.measureText(testLine).width;
              if (testWidth > maxWidth) {
                context.fillText(line, marginLeft, marginTop);
                line = words[n] + ' ';
                marginTop += lineHeight;
              } else {
                line = testLine;
              }
            }
            context.fillText(line, marginLeft, marginTop);
            marginTop += lineHeight;
          });

        };
        wrapText(ctx, element.value, pos.left, pos.top + 18, 250, 15);
      }
      element.remove();
    }
    $('#modal-body')[0].onclick = null;
  }

  saveImg() {
    const canvas = $('#imgCanvas')[0];
    this.currentImg.body = canvas.toDataURL().split(",")[1];
    this.imagesService.updateImage(this.currentImg)
      .subscribe(() => this.close());
  }

}
