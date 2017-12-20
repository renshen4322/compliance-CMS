app.factory("recordResource", function ($http) {
  return {
    getById: function (id) {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetById?id=" + id);
    },
    createRecord: function (record) {
      return $http.post("backoffice/SEACompliance/RIRecordApi/CreateRecord", angular.toJson(record));
    },
    getAll: function () {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetAll");
    },
    getRecordByKeyWordWithPage: function (pagerModel) {
      return $http.post("backoffice/SEACompliance/RIRecordApi/RecordByKeyWordWithPage", angular.toJson(pagerModel));
    },
    updateRecord: function (record) {
      return $http.post("backoffice/SEACompliance/RIRecordApi/UpdateRecord", angular.toJson(record));
    },
    deleteRecordById: function (id) {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetDeleteRecordById?id=" + id);
    },
    getTypeModule: function () {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetAllType_Module");
    },
    getTypeTopic: function (_p) {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetType_Topic?pcode=" + _p);
    },
    getTypeSub: function (_p) {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetType_Sub?pcode=" + _p);
    },
    GetAllType: function () {
      return $http.get("backoffice/SEACompliance/RIRecordApi/GetAllType");
    },
    AddorUpdateRecordType: function (record) {
      return $http.post("backoffice/SEACompliance/RIRecordApi/AddorUpdateRecordType", angular.toJson(record));
    },
    // record.edit.controller.js
    getDocumentsList: function (docId) {
      return $http.get("backoffice/SEACompliance/RlFileApi/GetAllFiles", {
        params: {
          documentId: docId
        }
      });
    },
    // editDocumentsCtrl
    modifyDocumentsItem: function (itemObj) {
      return $http.post("backoffice/SEACompliance/RlFileApi/ModifyFileInfo", {
        docid: itemObj.docId,
        title: itemObj.title,
        content: itemObj.content
      });
    },
    deleteDocumentsItem: function (docId) {
      return $http.post("backoffice/SEACompliance/RlFileApi/DeleteFileInfo?docid=" + docId);
    },
    downDocumentsItem: function (docId) {
      return $http.get("backoffice/RlFileInfo/DownLoadFile", {
        params: {
          docid: docId
        }
      });
    },
    modifyDocumentsItemByUrl: function (itemObj) {
      return $http.post("backoffice/SEACompliance/RlFileApi/UploadFileByUrl", {
        path: itemObj.path,
        documentId: itemObj.documentId,
        title: itemObj.title,
        content: itemObj.content
      });
    },
    // editDocumentsCtrl
    saveNewVersion: function (record) {
      return $http.post("backoffice/SEACompliance/RIRecordApi/CreateRecordNewVersion", angular.toJson(record));
    }
  };
});