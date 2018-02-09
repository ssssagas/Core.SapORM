# Core.SapORM
基于Dapper封装sap.hana基础增删改的ORM框架

### 说明

#### 实体对象需继承Id,IsDelete属性来实现

* 基础方法查询单个根据Id来查询
* 所有查询只查询IsDelete为true的数据
* 所有删除为假删除只是变更IsDelete状态为false

