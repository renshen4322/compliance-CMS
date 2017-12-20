app.controller("editDocumentsCtrl", function(
  $scope,
  $modalInstance,
  items,
  FileUploader,
  recordResource
) {
  var uploader = ($scope.uploader = new FileUploader({
    url: "backoffice/RlFileInfo/UploadFile",
    formData: []
  }));
  var initUpload = function() {
    // FILTERS
    // a sync filter
    uploader.filters.push({
      name: "syncFilter",
      fn: function(item /*{File|FileLikeObject}*/, options) {
        // console.log("syncFilter");
        return this.queue.length < 2;
      }
    });
    // an async filter
    uploader.filters.push({
      name: "asyncFilter",
      fn: function(item /*{File|FileLikeObject}*/, options, deferred) {
        // console.log("asyncFilter");
        setTimeout(deferred.resolve, 1e3);
      }
    });
    // 验证是否已经加入队列
    // uploader.filters.push({
    //   name: 'isHasFilter',
    //   fn: function (item /*{File|FileLikeObject}*/, options) {
    //     for (var i = 0; i < uploader.queue.length; i++) {
    //       var queueItem = uploader.queue[i];
    //       if (queueItem.file.name === item.name) {
    //         return false;
    //       }
    //     };
    //     return true
    //   }
    // });
    // 数据格式验证
    uploader.filters.push({
      name: "imageFilter",
      fn: function(item /*{File|FileLikeObject}*/, options) {
        // var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
        // return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        return true;
      }
    });
    uploader.filters.push({
      name: "isExcelOrWord",
      fn: function(item /*{File|FileLikeObject}*/, options) {
        var type = '|' + item.name.slice(item.name.lastIndexOf('.') + 1) + '|';
        $scope.urlError = false;
        if ('|ppt|pptx|xls|xlsx|doc|docx|pdf|txt|jpg|png|'.indexOf(type) !== -1) {
          $scope.uploadError = false;
          return true;
        } else {
          $scope.uploadError = true;
          return false;
        }
      }
    });
    // CALLBACKS
    uploader.onWhenAddingFileFailed = function(
      item /*{File|FileLikeObject}*/,
      filter,
      options
    ) {
      // console.info("onWhenAddingFileFailed", item, filter, options);
    };
    uploader.onAfterAddingFile = function(fileItem) {
      if (uploader.queue.length > 1) {
        uploader.queue.splice(0, 1);
      }
      reloadFilelist();
      // console.info("onAfterAddingFile", fileItem);
    };
    uploader.onAfterAddingAll = function(addedFileItems) {
      // console.info("onAfterAddingAll", addedFileItems);
    };
    uploader.onBeforeUploadItem = function(item) {
      // item.formData.push({
      //   documentId: $scope.editData.documentId,
      //   title: $scope.editData.title,
      //   content: $scope.editData.content
      // });
      // console.info("onBeforeUploadItem", item);
    };
    uploader.onProgressItem = function(fileItem, progress) {
      // console.info("onProgressItem", fileItem, progress);
    };
    uploader.onProgressAll = function(progress) {
      // console.info("onProgressAll", progress);
    };
    // 上传成功时
    uploader.onSuccessItem = function(fileItem, response, status, headers) {
      if (response.status === "success") {
        $scope.editData.uploadResponse = response.data;
        $modalInstance.close("ok");
      }
      // console.info('onSuccessItem');
    };
    uploader.onErrorItem = function(fileItem, response, status, headers) {
      // console.info("onErrorItem", fileItem, response, status, headers);
    };
    uploader.onCancelItem = function(fileItem, response, status, headers) {
      // console.info("onCancelItem", fileItem, response, status, headers);
    };
    uploader.onCompleteItem = function(fileItem, response, status, headers) {
      // console.info("onCompleteItem", fileItem, response, status, headers);
    };
    uploader.onCompleteAll = function() {
      // console.info("onCompleteAll");
    };
    // console.info("uploader", uploader);
  };
  var reloadFilelist = function() {
    var fileArray = [];
    for (var i = 0; i < uploader.queue.length; i++) {
      var item = uploader.queue[i];
      fileArray.push(item.file.name);
    }
    $scope.fileDocName = fileArray.join(";");
    $(".fileName").val($scope.fileDocName);
    // console.log($scope.fileDocName);
  };
  // 当用户在输入框进行文件的删除操作的时候
  $scope.changeFileList = function(fileDocName) {
    $scope.uploadError = false;
    var newfilelist = fileDocName;
    for (var i = 0; i < uploader.queue.length; i++) {
      var item = uploader.queue[i];
      if (fileDocName !== item.file.name) {
        uploader.clearQueue();
      }
    }
    $scope.fileDocName = newfilelist;
    $scope.fileRequired = false;
    if (isURL(fileDocName)) {
      $scope.urlError = false;
    } else {
      $scope.urlError = true;          
    }
  };
  var isURL = function(str) {
    var pattern = new RegExp(
      /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/
    ); // fragment locator
    return pattern.test(str);
  };
  // 数据验证
  var verificationData = function(type) {
    if (!$scope.fileDocName) {
      $scope.fileRequired = true;
      return false;
    }
  };
  // 保存url数据的时候
  var saveUrlData = function() {
    if (isURL($scope.fileDocName)) {
      $scope.urlError = false;
      $scope.editData.path = $scope.fileDocName;
      recordResource
        .modifyDocumentsItemByUrl($scope.editData)
        .then(function(response) {
          if (response.data.status === "success") {
            $modalInstance.close("ok");
          }
        });
    } else {
      $scope.urlError = true;
    }
  };
  var newItemObj = function() {
    if (!$scope.fileDocName) {
      $scope.fileRequired = true;
      return;
    }
    // 选择文件时
    if (uploader.queue.length > 0) {
      uploader.queue[0].formData.push({
        documentId: $scope.editData.documentId,
        title: $scope.editData.title,
        content: $scope.editData.content
      });
      uploader.uploadAll();
    } else {
      // 填写URL时
      saveUrlData();
    }
  };
  $scope.ok = function(valid) {
    if (!valid) {
      return
    }
    if (items.type === "add") {
      newItemObj();
    } else {
      recordResource
        .modifyDocumentsItem($scope.editData)
        .then(function(response) {
          if (response.data.status === "success") {
            $modalInstance.close("ok");
          }
        });
    }
  };
  $scope.cancel = function() {
    // uploader.clearQueue()
    $modalInstance.dismiss("cancel");
  };
  var init = function() {
    if (items.type === "add") {
      // new doc
      $scope.editData = {
        documentId: items.documentId
      };
      initUpload();
    } else {
      // edit
      $scope.editData = angular.copy(items);
      if (items.mimeType === "url") {
        $scope.fileDocName = items.path;
      } else {
        $scope.fileDocName = $scope.editData.fileName;
      }
    }
  };
  init();
});
