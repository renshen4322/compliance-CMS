function recordController($scope, recordResource, $location, notificationsService, $routeParams) {
    $scope.getAllRecord = function () {
        recordResource.getAll().then(function (response) {
            if (response.data.status === "success") {
                $scope.recordItems = response.data.data;
            }
        });
    }

    $scope.recordPagerModel = { CurrentPage: $routeParams.pageNumber > 0 ? $routeParams.pageNumber : 1 };
    $scope.itemsPerPage = 10;

    var recordsearchinfo = sessionStorage.getItem('recordsearchinfo') === null ? null : JSON.parse(decodeURIComponent(sessionStorage.getItem('recordsearchinfo')));
    $scope.recordSearchWord = recordsearchinfo === null ? { keyWord: '' } : { keyWord: recordsearchinfo.keyWord };

    $scope.fetchData = function (pageNumber) {
        getRecordList(pageNumber);
        sessionStorage.setItem('recordsearchinfo', encodeURIComponent(JSON.stringify({
            keyWord: $scope.recordSearchWord.keyWord
        })));
    }

    function getRecordList(pageNumber) {
        recordResource.getRecordByKeyWordWithPage({ CurrentPage: pageNumber, ItemsPerPage: $scope.itemsPerPage, KeyWord: $scope.recordSearchWord.keyWord }).then(function (response) {
            if (response.data.status === "success") {
                $scope.recordPagerModel = response.data.data;
            }
        });     

        //recordResource.getAll().then(function (response) {
        //    if (response.data.status === "success") {
        //        $scope.recordItems = response.data.data;
        //    }
        //});
    }

    $scope.location = function (id) {
        if (id) {
            $location.path('SEACompliance/SEACompliance/RIRecordEdit/' + id);
        }
        else
        {
            $location.path('SEACompliance/SEACompliance/RIRecordEdit/-1');
        }
        $location.search({ pageNumber: $scope.recordPagerModel.CurrentPage });
    }

    $scope.deleteRecordById = function (record) {
        if (confirm('Do you want to delete the record ?')) {
            recordResource.deleteRecordById(record.DocID).then(function (response) {
                if (response.data.status === "success") {
                    $scope.fetchData($scope.recordPagerModel.CurrentPage);
                    notificationsService.success("Success", record.DocID + " has been deleted! ");
                } else {
                    notificationsService.error("Failed", record.DocID + " deleted failed! ");
                }
            });
        }
    }

    $scope.setPage = function (pageNumber) {
        if ($scope.recordPagerModel.CurrentPage != pageNumber) {
            $scope.fetchData(pageNumber);
        }
    }

    $scope.prevPage = function () {
        if ($scope.recordPagerModel.CurrentPage > 1) {
            $scope.fetchData(--$scope.recordPagerModel.CurrentPage);
        }
    }

    $scope.nextPage = function () {
        if ($scope.recordPagerModel.CurrentPage < $scope.recordPagerModel.TotalPages) {
            $scope.fetchData(++$scope.recordPagerModel.CurrentPage);
        }
    }
}

angular.module("umbraco").controller('recordController', recordController);
