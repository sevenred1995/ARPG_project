using System.Collections.Generic;
using TaiDouCommon.Model;
using UnityEngine;


public delegate void OnAsyncTaskCompleteEvent();


public delegate void OnGetRoleEvent(List<Role> roleList);
public delegate void OngetEmptyRoleEvent();
public delegate void OnAddRoleEvent(Role role); 
public delegate void OnSelectRoleEvent();

public delegate void OnGetTaskListEvent(List<TaskDB> list);
public delegate void OnAddTaskEvent(TaskDB taskdb);
public delegate void OnUpdateTaskEvent();

public delegate void OnGetInventoryItemDBEvent(List<InventoryItemDB> list);
public delegate void OnAddInventoryItemDBEvent(InventoryItemDB itemDB);
public delegate void OnUpdateInventoryItemDBEvent();
public delegate void OnUpdateInventoryItemDBListEvent();

public delegate void OnGetTemEvent(List<Role> rolelist,int masterRoleID);
public delegate void OnwaitTeamEvent();
public delegate void OnCancelTeamEvent();

public delegate void OnAsyncPostionAndRotationEvent(int roleID,Vector3 pos,Vector3 eulerAnglers);
public delegate void OnAsyncPlayerMoveAnimationEvent(int roleID,PlayerMoveAnimationModel model);

public delegate void OnCreateEnemyEvent(CreateEnemyModel model);

public delegate void OnAsyncEnemyPostionRotationEvent(EnemyPostionModel model);
public delegate void OnSyncEnemyAnimationEvent(EnemyAnimationModel model);
