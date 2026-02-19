using DPK.Foundation.Domain.Entities;
using DPK.Foundation.EntityFrameworkCore.Repositories;

namespace DPK.Mosavabat.EntityFrameworkCore.EntityFrameworkCore.Repositories;

/// <summary>
///کلاس پایه برای ریپوزیتوری
/// </summary>
/// <typeparam name="TEntity">نام جدول در دیتا بیس</typeparam>
/// <typeparam name="TPrimaryKey">نوع کلید اصلی </typeparam>
public abstract class MosavabatRepositoryBase<TEntity, TPrimaryKey>(
    IDbContextProvider<MosavabatDbContext> dbContextProvider) :
    EfCoreRepositoryBase<MosavabatDbContext, TEntity, TPrimaryKey>(dbContextProvider)
    where TEntity : class, IEntity<TPrimaryKey>
{
    //در صورتی که نیاز دارید تابع کلی برای تمامی ریپوزیتوریها اضافه نمایید در این قسمت تابع خود را وارد نمایید
}

/// <summary>
/// کلاس پایه را به صورت پیش فرض اینت قراردادیم 
/// در این بخش شما می توانید هر نوع شامل long-Guid-.. نیز استفاده نمایید
/// This is a shortcut of <see cref="MosavabatRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public abstract class MosavabatRepositoryBase<TEntity>(IDbContextProvider<MosavabatDbContext> dbContextProvider)
    : MosavabatRepositoryBase<TEntity, int>(dbContextProvider)
    where TEntity : class, IEntity<int>
{
    //در این بخش به هیچ عنوان تابع ایی اضافه ننمایید
    //در صورت نیاز در کلاس اصلی استفاده نمایید
}