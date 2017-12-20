app.controller('recordController', function ($scope, recordResource, $location, notificationsService, $routeParams, CONFIRM) {
  //$scope.getAllRecord = function () {
  //    recordResource.getAll().then(function (response) {
  //        if (response.data.status === "success") {
  //            $scope.recordItems = response.data.data;
  //        }
  //    });
  //}
  $scope.recordPagerModel = { CurrentPage: $routeParams.pageNumber > 0 ? $routeParams.pageNumber : 1 };
  $scope.itemsPerPage = 10;

  var recordsearchinfo = sessionStorage.getItem('recordsearchinfo') === null ? null : JSON.parse(decodeURIComponent(sessionStorage.getItem('recordsearchinfo')));
  $scope.recordSearchWord = recordsearchinfo === null ? { keyWord: '' } : { keyWord: recordsearchinfo.keyWord };

  $scope.fetchData = function (pageNumber) {
    getRecordList(pageNumber);
    sessionStorage.setItem('recordsearchinfo', encodeURIComponent(JSON.stringify({
      keyWord: $scope.recordSearchWord.keyWord
    })));
  };
  // 格式化时间
  var formatDate = function (curDate) {
    // curDate:2017-09-30T10:19:44.137Z
    var date = new Date(curDate);
    // from your output, it seems that you want to get UTC time
    var result = date.getUTCFullYear() + "-" + (date.getUTCMonth() + 1) + "-" + date.getUTCDate()
      + " " + date.getUTCHours() + ":" + date.getUTCMinutes() + ":" + date.getUTCSeconds();
    return result;
  };
  function getRecordList(pageNumber) {
    recordResource.getRecordByKeyWordWithPage({ CurrentPage: pageNumber, ItemsPerPage: $scope.itemsPerPage, KeyWord: $scope.recordSearchWord.keyWord }).then(function (response) {
      if (response.data.status === "success") {
        for (var i = 0; i < response.data.data.Items.length; i++) {
          var item = response.data.data.Items[i];
          item.formatDate = formatDate(item.UPDATETIME);
        };
        $scope.recordPagerModel = response.data.data;
      }
    });
  };

  $scope.location = function (id) {
    if (id) {
      $location.path('SEACompliance/SEACompliance/RIRecordEdit/' + id);
    } else {
      $location.path('SEACompliance/SEACompliance/RIRecordEdit/-1');
    }
    $location.search({ pageNumber: $scope.recordPagerModel.CurrentPage });
  };

  $scope.deleteRecordById = function (record) {
    CONFIRM.open('Prompt', 'Do you want to delete the record ?').result.then(function (res) {
      if (res) {
        recordResource.deleteRecordById(record.DocID).then(function (response) {
          if (response.data.status === "success") {
            $scope.fetchData($scope.recordPagerModel.CurrentPage);
            notificationsService.success("Success", record.DocID + " has been deleted! ");
          } else {
            notificationsService.error("Failed", record.DocID + " deleted failed! ");
          }
        });
      }
    }, function () {

    });
  };

  $scope.setPage = function (pageNumber) {
    if ($scope.recordPagerModel.CurrentPage != pageNumber) {
      $scope.fetchData(pageNumber);
    }
  };

  $scope.prevPage = function () {
    if ($scope.recordPagerModel.CurrentPage > 1) {
      $scope.fetchData(--$scope.recordPagerModel.CurrentPage);
    }
  };

  $scope.nextPage = function () {
    if ($scope.recordPagerModel.CurrentPage < $scope.recordPagerModel.TotalPages) {
      $scope.fetchData(++$scope.recordPagerModel.CurrentPage);
    }
  };
});
