app.controller("recordEditController", function (
  $scope,
  CONSTANTS,
  $log,
  recordResource,
  $routeParams,
  $location,
  notificationsService,
  overlayHelper,
  formHelper,
  eventsService,
  $modal,
  CONFIRM
) {
  $scope.toggled = function (open) {
    $log.log("Dropdown is now: ", open);
  };
  $scope.toggleDropdown = function ($event) {
    $event.preventDefault();
    $event.stopPropagation();
    $scope.status.isopen = !$scope.status.isopen;
  };
  var getDocumentsList = function () {
    recordResource.getDocumentsList($routeParams.id).then(function (response) {
      if (response.data.status === "success") {
        if (response.data.data === null) {
          $scope.documentsList = [];
        } else {
          $scope.documentsList = response.data.data;
        }
      }
    });
  };
  // 删除某个documentItem
  $scope.deleteDocumentsItem = function (docId) {
    CONFIRM.open("prompt", "Are you sure to delete?").result.then(
      function (res) {
        recordResource.deleteDocumentsItem(docId).then(function (response) {
          if (response.data.status === "success") {
            getDocumentsList();
          }
        });
      },
      function () {}
    );
  };
  // 下载文件
  $scope.downFile = function (docItem) {
    var url = "";
    if (docItem.mimeType === "url") {
      url = docItem.path;
    } else {
      url = "backoffice/RlFileInfo/DownLoadFile?docid=" + docItem.docId;
    }
    window.open(url, "_blank");
  };
  $scope.editDoclistItem = function (docItem, index) {
    var ItemObj = {
      type: "add",
      documentId: $routeParams.id
    };
    if (docItem) {
      docItem.$index = index;
      ItemObj = docItem;
    }
    var modalInstance = $modal.open({
      animation: true,
      templateUrl: CONSTANTS.uibase + "record/editDocuments.html",
      controller: "editDocumentsCtrl",
      resolve: {
        items: function () {
          return ItemObj;
        }
      }
    });
    modalInstance.result.then(
      function (docItemObj) {
        getDocumentsList();
      },
      function () {
        $log.info("Modal dismissed at: " + new Date());
      }
    );
  };
  // 先分组，在添加序列号
  var serialNumber = function (checkItemList) {
    var result = [],
      hash = {};
    for (var i = 0; i < checkItemList.length; i++) {
      var elem = checkItemList[i].EntityID;
      if (!hash[elem]) {
        result.push(checkItemList[i]);
        hash[elem] = true;
      }
    }
    for (var index = 0; index < result.length; index++) {
      var element = result[index];
      element.serialNumber = index + 1;
      for (var j = 0; j < checkItemList.length; j++) {
        var item = checkItemList[j];
        if (element.EntityID === item.EntityID) {
          item.serialNumber = element.serialNumber;
        }
      }
    }
  };
  // 获取最大版本号,每一个Item都要加上(IsLatest老数据中没有这个字段，不能作为删选条件)
  var maxVersion = function (checkItemList) {
    // 先按照EntityID分类,去除重复的EntityID
    var entityIDList = [],
      hash = {};
    for (var i = 0; i < checkItemList.length; i++) {
      var elem = checkItemList[i].EntityID;
      if (!hash[elem]) {
        entityIDList.push(checkItemList[i].EntityID);
        hash[elem] = true;
      }
    }
    // 分类后的数据进行数据筛选，选出相同的EntityID的数据
    for (var i = 0; i < entityIDList.length; i++) {
      var tempList = [];
      var entity = entityIDList[i];
      // 先把EntityID放入临时数组
      for (var j = 0; j < checkItemList.length; j++) {
        var item = checkItemList[j];
        if (entity === item.EntityID) {
          tempList.push(item.Version);
        }
      }
      // 得出数组中最大的版本号
      var maxNum = Math.max.apply(null, tempList);
      // 在把所有EntityID相同的数据全部放入最大版本号
      for (var k = 0; k < checkItemList.length; k++) {
        if (entity === checkItemList[k].EntityID) {
          checkItemList[k].maxVersion = maxNum;
        }
      }
    }
  };
  // checkItem多版本的时候，只显示第一版
  var showCheckItem = function (checkItemList) {
    serialNumber(checkItemList);
    maxVersion(checkItemList);
    for (var i = 0; i < checkItemList.length; i++) {
      var item = checkItemList[i];
      item.index = i;
      if (item.UnderReview === null) {
        item.UnderReview = 0;
      };
      if (item.Version !== item.maxVersion) {
        item.isHide = true;
      }
    }
  };
  // 切换版本时查找相应的数据，并返回索引
  // curIndex是点击的checkitem的索引
  $scope.findCurVersionItem = function (EntityID, Version, curIndex) {
    for (var i = 0; i < $scope.recordPageModel.CheckItem.length; i++) {
      var item = $scope.recordPageModel.CheckItem[i];
      if (EntityID === item.EntityID && Version === item.Version) {
        $scope.recordPageModel.CheckItem[curIndex].isHide = true;
        delete $scope.recordPageModel.CheckItem[i].isHide;
      }
    }
  };
  // 循环版本列表
  $scope.getVersionNumber = function (num) {
    if (num > 0) {
      return new Array(num);
    }
  };

  function getById(id) {
    recordResource.getById(id).then(function (response) {
      if (response.data.data) {
        $scope.recordPageModel = response.data.data;
        generateEditorProperties();
        $scope.oldCheckItemNum = $scope.recordPageModel.CheckItem.length;
        getCheckItem($scope.recordPageModel.CheckItem);
        showCheckItem($scope.recordPageModel.CheckItem);
        // setTimeout($scope.versionDiff(JSON.parse(sessionStorage.getItem('curItem'))),1000);        
        // $scope.modifyCheckItem($scope.recordPageModel.CheckItem[0]);
        //alert($scope.recordPageModel.PackageID);
        if (
          $scope.recordPageModel.PackageID != undefined &&
          $scope.recordPageModel.PackageID != "" &&
          $scope.recordPageModel.PackageID != null
        ) {
          gettopictypes($scope.recordPageModel.PackageID); //页面加载时二级分类重新加载一下
        }
        if (
          $scope.recordPageModel.Topic != undefined &&
          $scope.recordPageModel.Topic != "" &&
          $scope.recordPageModel.Topic != null
        ) {
          getsubtypes($scope.recordPageModel.Topic); //页面加载时三级分类重新加载一下
        }
      }
    });
  }
  $scope.validateTitleText = function (title) {
    if ($("#Title").val().length <= 0) {
      $scope.validationFields["titleText"].isValid = false;
      $scope.validationFields["titleText"].title = "Title is a required field";
      return false;
    } else {
      $scope.validationFields["titleText"].isValid = true;
      $scope.validationFields["titleText"].title = "";
    }
    return true;
  };
  $scope.validateAuthorText = function (Author) {
    if ($("#Author").val().length <= 0) {
      $scope.validationFields["authorText"].isValid = false;
      $scope.validationFields["authorText"].title =
        "Author is a required field";
      return false;
    } else {
      $scope.validationFields["authorText"].isValid = true;
      $scope.validationFields["authorText"].title = "";
    }
    return true;
  };

  function getCheckItem(checkItemList) {
    if (checkItemList) {
      $scope.listItems = [];
      $.each(checkItemList, function (index, item) {
        var itemlist = {
          Importance: [{
              Id: 1,
              Text: "Importance Degree1"
            },
            {
              Id: 2,
              Text: "Importance Degree2"
            },
            {
              Id: 3,
              Text: "Importance Degree3"
            },
            {
              Id: 4,
              Text: "Importance Degree4"
            },
            {
              Id: 5,
              Text: "Importance Degree5"
            }
          ],
          HasMeta: [{
            Id: "N",
            Text: "OFF"
          }, {
            Id: "Y",
            Text: "ON"
          }],
          CheckItemContent: [{
            label: "RichText",
            description: "RichText",
            view: "rte",
            config: propertyItemConfig,
            hideLabel: true,
            value: item.CheckItemContent
          }],
          Penalty: [{
            label: "RichText",
            description: "RichText",
            view: "rte",
            config: propertyItemConfig,
            hideLabel: true,
            value: item.Penalty
          }],
          ReasonCodes: [{
            label: "RichText",
            description: "RichText",
            view: "rte",
            config: propertyItemConfig,
            hideLabel: true,
            value: item.ReasonCodes
          }],
          ImportanceSelectedVaule: [{
            value: item.Importance
          }],
          HasMetaSelectedVaule: [{
            value: item.HasMeta
          }],
          DocID: item.DocID,
          EntityID: item.EntityID,
          Version: item.Version
        };
        $scope.listItems.push(itemlist);
      });
    }
  }
  // 新增checkitem
  $scope.modifyCheckItem = function (CheckItemObj, index) {
    var ItemData = function () {
      if (!CheckItemObj) {
        var tempData = {
          type: "add",
          routeParamsId: $routeParams.id
        };
        return tempData;
      } else {
        CheckItemObj.$index = index;
        CheckItemObj.routeParamsId = $routeParams.id;
        return CheckItemObj;
      }
    };
    var checkItemInstance = $modal.open({
      animation: true,
      templateUrl: CONSTANTS.uibase + "record/editCheckItem.html",
      controller: "editCheckItemCtrl",
      resolve: {
        items: function () {
          return ItemData();
        }
      }
    });
    checkItemInstance.result.then(
      function (editObj) {
        // Version: 当前Item的版本
        // maxVersion: 所有的Item都要带上最大的版本号才能渲染出正确的版本列表
        // serialNumber: 当前Item属于哪一个序列,
        // index: 当前Item在$scope.recordPageModel.CheckItem数组中的索引记录
        if (editObj.isNewCheckItemVersion) {
          // 新建的doc只能进行保存，任何状态下都不会新建版本
          if ($routeParams.id > 0) {
            $scope.isCheckItemNew = true;
            $("#saveBut").css({
              cursor: "not-allowed"
            });
          }
          // checkItem保存新版本時，DocId為空
          $scope.recordPageModel.CheckItem.push({
            DocID: "",
            HasMeta: "Y",
            EntityID: editObj.EntityID,
            Importance: editObj.Importance,
            ReasonCodes: editObj.reasonCodes[0].value,
            Penalty: editObj.penalty[0].value,
            CheckItemContent: editObj.checkItemContent[0].value,
            Version: editObj.maxVersion + 1,
            maxVersion: editObj.maxVersion + 1,
            serialNumber: editObj.serialNumber,
            index: $scope.recordPageModel.CheckItem.length,
            UnderReview: editObj.UnderReview
          });
          // 每次编辑完并且生成checkitem的新版本的时候都要刷新maxVersion。防止下拉菜单报错
          for (var i = 0; i < $scope.recordPageModel.CheckItem.length; i++) {
            var item = $scope.recordPageModel.CheckItem[i];
            if (item.EntityID === editObj.EntityID) {
              item.maxVersion = editObj.maxVersion + 1;
            }
          }
          $scope.recordPageModel.CheckItem[editObj.index].isHide = true;
          return;
        }
        if (editObj.type === "add") {
          if ($routeParams.id > 0) {
            $scope.isCheckItemNew = true;
            $("#saveBut").css({
              cursor: "not-allowed"
            });
          }
          // 算出当前最大的序列号，并追加新的序列号
          var serialNumberList = [];
          for (var i = 0; i < $scope.recordPageModel.CheckItem.length; i++) {
            var item = $scope.recordPageModel.CheckItem[i];
            serialNumberList.push(item.serialNumber);
          }
          // 没有数据的情况下，maxSerialNumber为0
          var maxSerialNumber = 0;
          if (serialNumberList.length > 0) {
            maxSerialNumber = Math.max.apply(null, serialNumberList);
          }
          // 编辑doc的状态下，新增checkItem时doc必须新建版本
          $scope.recordPageModel.CheckItem.push({
            DocID: "",
            EntityID: new Date().getTime(),
            HasMeta: "Y",
            Importance: editObj.Importance,
            ReasonCodes: editObj.reasonCodes[0].value,
            Penalty: editObj.penalty[0].value,
            CheckItemContent: editObj.checkItemContent[0].value,
            Version: 1,
            maxVersion: 1,
            serialNumber: maxSerialNumber + 1,
            index: $scope.recordPageModel.CheckItem.length,
            UnderReview: editObj.UnderReview
          });
        } else {
          // 普通编辑后保存
          var curItem = $scope.recordPageModel.CheckItem[editObj.index];
          curItem.Importance = editObj.Importance;
          curItem.ReasonCodes = editObj.reasonCodes[0].value;
          curItem.Penalty = editObj.penalty[0].value;
          curItem.CheckItemContent = editObj.checkItemContent[0].value;
          curItem.UnderReview = editObj.UnderReview;
        }
      },
      function () {
        $log.info("Modal dismissed at: " + new Date());
      }
    );
  };
  // 只有编辑document状态下才使用，不处理新增的document
  // 删除checkItem后检查数据是否删除完所有新增的数据，如果没有，开启save按钮
  // 新增的数据DocID都为空
  var showSaveButton = function () {
    var len = $scope.recordPageModel.CheckItem.length;
    var newCheckItemNum = 0;
    for (var index = 0; index < len; index++) {
      var element = $scope.recordPageModel.CheckItem[index];
      if (element.DocID === "") {
        newCheckItemNum++
      }
    };
    if (newCheckItemNum === 0 && $scope.oldCheckItemNum === len) {
      $scope.isCheckItemNew = false;
      $("#saveBut").css({
        cursor: "pointer"
      });
    } else {
      $scope.isCheckItemNew = true;
      $("#saveBut").css({
        cursor: "not-allowed"
      });
    }
  };
  // 删除某一个checkItem
  $scope.deleteCheckItem = function (checkItem, $index) {
    CONFIRM.open("prompt", "Are you sure to delete?").result.then(
      function (res) {
        for (
          var i = 0, flag = true, len = $scope.recordPageModel.CheckItem.length; i < len; flag ? i++ : i
        ) {
          if (
            $scope.recordPageModel.CheckItem[i] &&
            checkItem.EntityID === $scope.recordPageModel.CheckItem[i].EntityID
          ) {
            $scope.recordPageModel.CheckItem.splice(i, 1);
            flag = false;
          } else {
            flag = true;
          }
        }
        // 重新计算下标，并且重新计算序列号
        for (var i = 0; i < $scope.recordPageModel.CheckItem.length; i++) {
          var item = $scope.recordPageModel.CheckItem[i];
          item.index = i;
        }
        if ($routeParams.id > 0) {
          showSaveButton();
        }
        serialNumber($scope.recordPageModel.CheckItem);
      },
      function () {}
    );
  };
  // $scope.DeleteCheckItem = function (idx) {
  //   $scope.listItems.splice(idx, 1);
  // };
  function generateEditorProperties() {
    var propertyConfig = {
      editor: {
        toolbar: [
          // "code",
          "undo",
          "redo",
          "cut",
          "bold",
          "italic",
          "underline",
          // "alignleft",
          // "aligncenter",
          // "alignright",
          // "bullist",
          // "numlist",
          // "table",
          "link",
          "fontselect",
          "fontsizeselect",
          "forecolor"
        ],
        stylesheets: [],
        dimensions: {
          height: 140,
          width: "90%"
        }
      }
    };

    $scope.summaryProperties = [{
      label: "RichText",
      description: "RichText",
      view: "rte",
      config: propertyConfig,
      hideLabel: true,
      value: $scope.recordPageModel.Summary ?
        $scope.recordPageModel.Summary : ""
    }];

    $scope.riskProperties = [{
      label: "RichText",
      description: "RichText",
      view: "rte",
      config: propertyConfig,
      hideLabel: true,
      value: $scope.recordPageModel.Risk ? $scope.recordPageModel.Risk : ""
    }];

    $scope.referenceDocumentProperties = [{
      label: "RichText",
      description: "RichText",
      view: "rte",
      config: propertyConfig,
      hideLabel: true,
      value: $scope.recordPageModel.ReferenceDocument ?
        $scope.recordPageModel.ReferenceDocument : ""
    }];
  }
  // 轮询开启监听
  var watchFunc = function (watchVaule, index, className) {
    var watchContent = $scope.$watch(watchVaule, function (newValue, oldValue) {
      if (newValue != undefined && oldValue != undefined) {
        if (newValue) {
          RemoveValidateDiv(className + index);
          watchContent();
        }
      }
    });
  };
  // 通用的验证方法
  var CheckItemFuc = function (className) {
    for (var i = 0; i < $scope.listItems.length; i++) {
      if (!$scope.listItems[i][className][0].value) {
        ValidateDiv(className + i);
        var watchStr = "listItems[" + i + "]." + className + "[0].value";
        watchFunc(watchStr, i, className);
        return false;
      }
    }
    return true;
  };
  // 获取所有已上传文档的docId
  var getAllDocId = function () {
    var docIdList = [];
    for (var i = 0; i < $scope.documentsList.length; i++) {
      var doc = $scope.documentsList[i];
      docIdList.push(doc.docId);
    }
    return docIdList;
  };
  // 保存新的版本
  $scope.saveNewVersion = function () {
    if (!$scope.validateTitleText()) {
      return false;
    }
    if (!$scope.validateAuthorText()) {
      return false;
    }
    $scope.recordPageModel.ReferenceDocument =
      $scope.referenceDocumentProperties[0].value;
    if (!$scope.summaryProperties[0].value) {
      ValidateDiv("txtSummary");
      return;
    } else if (!$scope.riskProperties[0].value) {
      ValidateDiv("txtRisk");
      return;
    }
    if ($scope.recordPageModel.CheckItem.length === 0) {
      notificationsService.warning(
        "Failed",
        "Please fill in the 'Check item' related data"
      );
      return;
    }
    var saveObj = {
      ID: $routeParams.id,
      DocID: $routeParams.id,
      Title: $scope.recordPageModel.Title,
      PackageID: $scope.recordPageModel.PackageID,
      Topic: $scope.recordPageModel.Topic,
      SubTopic: $scope.recordPageModel.SubTopic,
      Author: $scope.recordPageModel.Author,
      Summary: $scope.summaryProperties[0].value,
      Risk: $scope.riskProperties[0].value,
      ReferenceDocument: $scope.referenceDocumentProperties[0].value,
      EntityID: $scope.recordPageModel.EntityID,
      Version: $scope.recordPageModel.Version,
      IsLatest: $scope.recordPageModel.IsLatest,
      IsDelete: $scope.recordPageModel.IsDelete,
      CREATETIME: $scope.recordPageModel.CREATETIME,
      UPDATETIME: $scope.recordPageModel.UPDATETIME,
      Path: $scope.recordPageModel.Path,
      EffectiveDate: $scope.recordPageModel.EffectiveDate,
      MetaPath: $scope.recordPageModel.MetaPath,
      CheckItem: $scope.recordPageModel.CheckItem,
      CheckFiles: getAllDocId()
    };
    if ($routeParams.id > 0) {
      recordResource.saveNewVersion(saveObj).then(function (response) {
        if (response.data.status === "success") {
          $location.path("SEACompliance/SEACompliance/RIRecord/-1");
          notificationsService.success(
            "Success",
            $scope.recordPageModel.Title + " has been updated!"
          );
        } else {
          notificationsService.error("Failed", response.data.code);
        }
      });
    }
  };
  $scope.SaveRecord = function () {
    if (!$scope.validateTitleText()) {
      return false;
    }
    if (!$scope.validateAuthorText()) {
      return false;
    }
    $scope.recordPageModel.ReferenceDocument =
      $scope.referenceDocumentProperties[0].value;
    if (!$scope.summaryProperties[0].value) {
      ValidateDiv("txtSummary");
      return;
    } else if (!$scope.riskProperties[0].value) {
      ValidateDiv("txtRisk");
      return;
    }
    if ($scope.recordPageModel.CheckItem.length === 0) {
      notificationsService.warning(
        "Failed",
        "Please fill in the 'Check item' related data"
      );
      return;
    }
    var saveObj = {
      ID: $routeParams.id,
      DocID: $routeParams.id,
      Title: $scope.recordPageModel.Title,
      PackageID: $scope.recordPageModel.PackageID,
      Topic: $scope.recordPageModel.Topic,
      SubTopic: $scope.recordPageModel.SubTopic,
      Author: $scope.recordPageModel.Author,
      Summary: $scope.summaryProperties[0].value,
      Risk: $scope.riskProperties[0].value,
      ReferenceDocument: $scope.referenceDocumentProperties[0].value,
      EntityID: $scope.recordPageModel.EntityID,
      Version: $scope.recordPageModel.Version,
      IsLatest: $scope.recordPageModel.IsLatest,
      IsDelete: $scope.recordPageModel.IsDelete,
      CREATETIME: $scope.recordPageModel.CREATETIME,
      UPDATETIME: $scope.recordPageModel.UPDATETIME,
      Path: $scope.recordPageModel.Path,
      EffectiveDate: $scope.recordPageModel.EffectiveDate,
      MetaPath: $scope.recordPageModel.MetaPath,
      CheckItem: $scope.recordPageModel.CheckItem,
      CheckFiles: getAllDocId()
    };
    if ($routeParams.id > 0) {
      recordResource.updateRecord(saveObj).then(function (response) {
        if (response.data.status === "success") {
          $location.path("SEACompliance/SEACompliance/RIRecord/-1");
          notificationsService.success(
            "Success",
            $scope.recordPageModel.Title + " has been updated!"
          );
        } else {
          notificationsService.error("Failed", response.data.code);
        }
      });
    } else {
      //为了防止新建EntityID为空后，其他功能报错，创建时EntityID为当前时间，提交时必须设置为空
      var copySaveObj = angular.copy(saveObj);
      for (var i = 0; i < copySaveObj.CheckItem.length; i++) {
        copySaveObj.CheckItem[i].EntityID = "";
      }
      recordResource.createRecord(copySaveObj).then(function (response) {
        if (response.data.status === "success") {
          $location.path("SEACompliance/SEACompliance/RIRecord/-1");
          notificationsService.success("Success", "Record has been created!");
        } else {
          notificationsService.error("Failed", response.data.code);
        }
      });
    }
  };
  // 保存documents,,,最初版本备份
  $scope.SaveRecord2222222222 = function () {
    if (!$scope.validateTitleText()) {
      return false;
    }
    if (!$scope.validateAuthorText()) {
      return false;
    }

    // if (!ChkForm())
    // {
    //     return false;
    // }
    $scope.recordPageModel.Title = $scope.recordPageModel.Title;
    $scope.recordPageModel.Author = $scope.recordPageModel.Author;
    $scope.recordPageModel.Summary = $scope.summaryProperties[0].value;
    $scope.recordPageModel.Risk = $scope.riskProperties[0].value;
    $scope.recordPageModel.ReferenceDocument =
      $scope.referenceDocumentProperties[0].value;
    if (!$scope.recordPageModel.Summary) {
      ValidateDiv("txtSummary");
      return;
    } else if (!$scope.recordPageModel.Risk) {
      ValidateDiv("txtRisk");
      return;
    } else if (!$scope.recordPageModel.ReferenceDocument) {
      ValidateDiv("txtRefDoc");
      return;
    }
    // 验证CheckItem的数据
    // if (!CheckItemFuc('CheckItemContent')) {
    //   return
    // };
    // if (!CheckItemFuc('Penalty')) {
    //   return
    // };
    // if (!CheckItemFuc('ReasonCodes')) {
    //   return
    // };
    $scope.recordPageModel.ID = $routeParams.id;
    $scope.checkItemArray = new Array();
    for (var i in $scope.listItems) {
      $scope.userScoreModel = {
        Importance: "",
        HasMeta: "",
        CheckItemContent: "",
        Penalty: "",
        ReasonCodes: ""
      };
      if ($scope.listItems[i].SelectedValue == undefined) {
        $scope.userScoreModel.Importance =
          $scope.listItems[i].ImportanceSelectedVaule[0].value;
      } else {
        $scope.userScoreModel.Importance = $scope.listItems[i].SelectedValue;
      }
      if ($scope.listItems[i].SelectedHasMetaValue == undefined) {
        $scope.userScoreModel.HasMeta =
          $scope.listItems[i].HasMetaSelectedVaule[0].value;
      } else {
        $scope.userScoreModel.HasMeta =
          $scope.listItems[i].SelectedHasMetaValue;
      }
      $scope.userScoreModel.CheckItemContent =
        $scope.listItems[i].CheckItemContent[0].value;
      if ($scope.listItems[i].CheckItemContent[0].value == "") {
        alert("Plase Edit CheckItemContent");
        return false;
      }
      $scope.userScoreModel.Penalty = $scope.listItems[i].Penalty[0].value;
      if ($scope.listItems[i].Penalty[0].value == "") {
        alert("Plase Edit Penalty");
        return false;
      }
      $scope.userScoreModel.ReasonCodes =
        $scope.listItems[i].ReasonCodes[0].value;
      if ($scope.listItems[i].ReasonCodes[0].value == "") {
        alert("Plase Edit Relevant Provisions");
        return false;
      }
      $scope.userScoreModel.DocID = $scope.listItems[i].DocID;
      $scope.userScoreModel.EntityID = $scope.listItems[i].EntityID;
      $scope.userScoreModel.Version = $scope.listItems[i].Version;
      $scope.checkItemArray.push($scope.userScoreModel);
    }
    $scope.recordPageModel.CheckItem = $scope.checkItemArray;
    if ($routeParams.id > 0) {
      recordResource
        .updateRecord($scope.recordPageModel)
        .then(function (response) {
          if (response.data.status === "success") {
            $location.path("SEACompliance/SEACompliance/RIRecord/-1");
            notificationsService.success(
              "Success",
              $scope.recordPageModel.Title + " has been updated!"
            );
          } else {
            notificationsService.error("Failed", response.data.code);
          }
        });
    } else {
      recordResource
        .createRecord($scope.recordPageModel)
        .then(function (response) {
          if (response.data.status === "success") {
            $location.path("SEACompliance/SEACompliance/RIRecord/-1");
            notificationsService.success("Success", "Record has been created!");
          } else {
            notificationsService.error("Failed", response.data.code);
          }
        });
    }
  };
  //主目录分类
  function getmoduletypes() {
    recordResource.getTypeModule().then(function (response) {
      if (response.data.data) {
        $scope.moduletypes = response.data.data;
      }
    });
  }
  //主目录change事件
  $scope.Module_change = function (x) {
    //alert(x);
    gettopictypes(x);
  };
  //topic目录分类
  function gettopictypes(_p) {
    recordResource.getTypeTopic(_p).then(function (response) {
      if (response.data.data) {
        $scope.topictypes = response.data.data;
      }
    });
  }
  //topic目录change事件
  $scope.Topic_change = function (x) {
    getsubtypes(x);
  };
  //sub目录分类
  function getsubtypes(_p) {
    recordResource.getTypeSub(_p).then(function (response) {
      if (response.data.data) {
        $scope.subtypes = response.data.data;
      }
    });
  }
  //滚动到指定div位置
  function ValidateDiv(_scrollTo) {
    // $("#" + _scrollTo + "").addClass("invalidField");
    $("#" + _scrollTo + " iframe").css({
      border: "1px solid red"
    });
    var container = $("#recordId");
    scrollTo = $("#" + _scrollTo + "");
    container.scrollTop(
      scrollTo.offset().top - container.offset().top + container.scrollTop()
    );
  }

  function RemoveValidateDiv(_scrollTo) {
    // $("#" + _scrollTo + "").removeClass("invalidField");
    $("#" + _scrollTo + " iframe").css({
      border: "0px"
    });
  }
  // 监听是否有值
  $scope.$watch("summaryProperties[0].value", function (newValue, oldValue) {
    if (newValue != undefined && oldValue != undefined) {
      if (newValue) {
        RemoveValidateDiv("txtSummary");
      }
    }
  });
  $scope.$watch("riskProperties[0].value", function (newValue, oldValue) {
    if (newValue != undefined && oldValue != undefined) {
      if (newValue) {
        RemoveValidateDiv("txtRisk");
      }
    }
  });
  $scope.$watch("referenceDocumentProperties[0].value", function (
    newValue,
    oldValue
  ) {
    if (newValue != undefined && oldValue != undefined) {
      if (newValue) {
        RemoveValidateDiv("txtRefDoc");
      }
    }
  });
  var propertyItemConfig = {
    editor: {
      toolbar: [
        // "code",
        "undo",
        "redo",
        "cut",
        "bold",
        "italic",
        "underline",
        // "alignleft",
        // "aligncenter",
        // "alignright",
        // "bullist",
        // "numlist",
        // "table",
        "link",
        "fontselect",
        "fontsizeselect",
        "forecolor"
      ],
      stylesheets: [],
      dimensions: {
        height: 100,
        width: "90%"
      }
    }
  };
  // 当前版本和最新版本对比功能
  $scope.versionDiff = function(curItem) {
    // sessionStorage.setItem('curItem', JSON.stringify(curItem));
    var checkItemList = angular.copy($scope.recordPageModel.CheckItem);
    curItem.Importance = curItem.Importance.toString();
    var maxVersionItem = {};
    // 获取当前最大版本的Item
    for(var i=0; i < checkItemList.length; i++) {
      var item = checkItemList[i];
      if (item.EntityID === curItem.EntityID && item.Version === item.maxVersion) {
        maxVersionItem = item;
      }
    };
    $modal.open({
      animation: true,
      templateUrl: CONSTANTS.uibase + "record/diffCheckItem.html",
      controller: "diffCheckItemCtrl",
      resolve: {
        items: function () {
          return {
            maxVersionItem:maxVersionItem,
            curItem:curItem
          };
        }
      }
    });
  };
  var init = function () {
    $scope.documentsList = [];
    $scope.isCheckItemNew = false;
    $("#saveBut").css({
      cursor: "pointer"
    });
    $scope.items = [
      "The first choice!",
      "And another choice for you.",
      "but wait! A third!"
    ];
    $scope.status = {
      isopen: false
    };
    $scope.recordPageModel = {
      DocID: ""
    };
    $scope.validationFields = {
      titleText: {
        isValid: true,
        title: ""
      },
      authorText: {
        isValid: true,
        title: ""
      }
    };
    if ($routeParams.id > 0) {
      $scope.isShowSaveButton = true;
      getById($routeParams.id);
    } else {
      $scope.recordPageModel.CheckItem = [];
      $scope.isShowSaveButton = false;
      generateEditorProperties();
    }
    $scope.listItems = [];
    getDocumentsList();
    getmoduletypes();     
  };
  init();
});