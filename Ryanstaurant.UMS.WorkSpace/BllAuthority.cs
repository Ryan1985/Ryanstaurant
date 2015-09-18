﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ryanstaurant.UMS.DataAccess.EF;
using Ryanstaurant.UMS.DataContract;
using Ryanstaurant.UMS.DataContract.Utility;

namespace Ryanstaurant.UMS.WorkSpace
{
    public class BllAuthority:BllBase
    {

        public new UmsEntities Entities
        {
            get
            {
                return base.Entities;
            }
            set
            {
                base.Entities = value;
            }
        }


        public List<ItemContent> QueryAuthority(List<ItemContent> authorities)
        {
            if (authorities == null || authorities.Count == 0)
            {
                throw new Exception("没有找到需要的操作", new Exception("RequestContents为空"));
            }

            if (authorities.Count == 1 && authorities[0] == null)
            {
                throw new Exception("没有找到需要的操作", new Exception("RequestContents没有包含操作"));
            }


            var queries =
               (from e in authorities
                where e.RequestInfo != null && e.RequestInfo.Operation == RequestOperation.Query
                select e).ToList();

            return Query(!queries.Any() ? null : queries.ToList());
        }

        private List<ItemContent> Query(IEnumerable<ItemContent> itemContents)
        {
            var resultEntity = new List<ItemContent>();


                List<authority> authList;


                //没有指定，则返回所有查询结果
                if (itemContents == null)
                {
                    authList = (from e in Entities.authority select e).ToList();
                }
                else//有指定，则从传送的数据处进行查询
                {
                    var authorities = itemContents.Cast<Authority>().ToList();
                    var authIDList = (from e in authorities
                                     select e.ID).ToList();

                    var authNameList = (from e in authorities
                                        select e.Name).ToList();

                    authList =
                        (from e in Entities.authority
                            where authIDList.Contains(e.id) || authNameList.Contains(e.Name)
                            select e).ToList();
                }

                resultEntity.AddRange(authList.Select(auth => new Authority
                {
                    ResultInfo = new ResultContent
                    {
                        State = ResultState.Success,
                        Exception = string.Empty,
                        InnerErrorMessage = string.Empty
                    },
                    ID = auth.id,
                    Description = auth.Description,
                    Name = auth.Name
                }));


            return resultEntity;
        }


        public List<ItemContent> ExecuteAuthority(List<ItemContent> authorities)
        {
            if (authorities == null || authorities.Count == 0)
            {
                throw new Exception("没有找到需要的操作", new Exception("RequestContents为空"));
            }

            if (authorities.Count == 1 && authorities[0] == null)
            {
                throw new Exception("没有找到需要的操作", new Exception("RequestContents没有包含操作"));
            }


            var queries =
               (from e in authorities
                where e.RequestInfo != null && e.RequestInfo.Operation != RequestOperation.Query
                select e).ToList();

            if (!queries.Any())
            {
                throw new Exception("没有找到需要的操作", new Exception("RequestContents没有包含执行(add,modify或者delete)操作"));
            }

            var resultEntity = Save(authorities);

            return resultEntity;
        }

        internal List<ItemContent> Save(List<ItemContent> requestEntitiies)
        {
            var resultEntity = new List<ItemContent>();


                foreach (var content in requestEntitiies)
                {
                    var resultcontent = new ItemContent();


                    if (content == null)
                    {
                        resultcontent.ResultInfo.Exception = "未设置操作类型";
                        resultcontent.ResultInfo.InnerErrorMessage = "ResultContent为NULL";
                        resultcontent.ResultInfo.State = ResultState.Fail;

                    }
                    else
                    {
                        switch (content.RequestInfo.Operation)
                        {
                            case RequestOperation.Add:
                            {
                                resultcontent = AddAuthorities( content);
                            }
                                break;
                            case RequestOperation.Modify:
                            {
                                resultcontent = ModifyAuthorities( content);
                            }
                                break;
                            case RequestOperation.Delete:
                            {
                                resultcontent = DeleteAuthorities( content);
                            }
                                break;
                            default:
                            {
                                resultcontent.ResultInfo.Exception = "错误的操作类型";
                                resultcontent.ResultInfo.InnerErrorMessage = "RequestOperation=" +
                                                                             Enum.GetName(typeof (RequestOperation),
                                                                                 content.RequestInfo.Operation);
                                resultcontent.ResultInfo.State = ResultState.Fail;
                            }
                                break;
                        }
                    }
                    resultEntity.Add(resultcontent);

                
            }
            return resultEntity;
        }

        private ItemContent DeleteAuthorities(ItemContent content)
        {
            var authority = content as Authority;


            if (authority == null)
            {
                return new Authority
                {
                    ResultInfo = new ResultContent
                    {
                        Exception = "错误的数据类型，没有进行删除操作",
                        InnerErrorMessage = string.Empty,
                        State = ResultState.Success
                    }
                    
                };
            }



            var authorityInDb = (from e in Entities.authority where e.id == authority.ID select e).FirstOrDefault();

            if (authorityInDb == null)
            {
                return new Authority
                {
                    ResultInfo = new ResultContent
                    {

                        Exception = "ID为[" + authority.ID + "]的权限不存在，没有进行删除操作",
                        InnerErrorMessage = string.Empty,
                        State = ResultState.Success
                    },
                    ID = authority.ID,
                    Description = authority.Description,
                    Name = authority.Name
                };
            }


            //去除所有角色中的当前权限
            var roles = from e in Entities.role select e;

            foreach (var role in roles)
            {
                role.Authority &= ~authority.ID;
            }

            //去除所有人员中的当前权限
            var employees = from e in Entities.employee select e;
            foreach (var employee in employees)
            {
                employee.Authority &= ~authority.ID;
            }

            Entities.authority.Remove(authorityInDb);

            Entities.SaveChanges();


            return new Authority
            {
                ResultInfo = new ResultContent
                {

                    Exception = string.Empty,
                    InnerErrorMessage = string.Empty,
                    State = ResultState.Success
                },

                ID = authority.ID,
                Description = authority.Description,
                Name = authority.Name
            };
        }

        private ItemContent ModifyAuthorities(ItemContent content)
        {
            var authority = content as Authority;


            if (authority == null)
            {
                return new Authority
                {
                    ResultInfo = new ResultContent
                    {
                        Exception = "错误的数据类型，没有进行修改操作",
                        InnerErrorMessage = string.Empty,
                        State = ResultState.Success
                    }

                };
            }


            var authorityInDb = (from e in Entities.authority where e.id == authority.ID select e).FirstOrDefault();

            if (authorityInDb == null)
            {
                return new Authority
                {
                    ResultInfo = new ResultContent
                    {
                        Exception = "ID为[" + authority.ID + "]的权限不存在，没有进行修改操作",
                        InnerErrorMessage = string.Empty,
                        State = ResultState.Success
                    },
                    ID = authority.ID,
                    Description = authority.Description,
                    Name = authority.Name

                };
            }


            authorityInDb.Description = authority.Description;
            authorityInDb.Name = authority.Name;


            Entities.SaveChanges();


            return new Authority
            {
                ResultInfo = new ResultContent
                {

                    Exception = string.Empty,
                    InnerErrorMessage = string.Empty,
                    State = ResultState.Success
                },
                ID = authority.ID,
                Description = authority.Description,
                Name = authority.Name
            };
        }



        private long GetAvailableAuthId()
        {
            var idList = (from i in Entities.authority orderby i.id select i.id).ToList();
            for (long i = 1; i < long.MaxValue; i = i << 1)
            {
                if (idList.Contains(i)) continue;
                return i;
            }
            return 1;
        }





        private  ItemContent AddAuthorities(ItemContent content)
        {
            var authority = content as Authority;

            if (authority == null)
            {
                return new Authority
                {
                    ResultInfo = new ResultContent
                    {
                        Exception = "错误的数据类型，没有进行添加操作",
                        InnerErrorMessage = string.Empty,
                        State = ResultState.Success
                    }

                };
            }


            var authToAdd = new authority
            {
                id = GetAvailableAuthId(),
                Description = authority.Description,
                Name = authority.Name
            };

            Entities.authority.Add(authToAdd);

            Entities.SaveChanges();

            authority.ID = authToAdd.id;

            return new Authority
            {
                ResultInfo = new ResultContent
                {

                    Exception = string.Empty,
                    InnerErrorMessage = string.Empty,
                    State = ResultState.Success
                },
                ID = authority.ID,
                Description = authority.Description,
                Name = authority.Name
            };
        }


    }
}
