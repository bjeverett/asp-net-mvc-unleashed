﻿using System;
using System.Web.Mvc;
using UnleashedBlog.Models.EntityFramework;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnleashedBlog.Models
{
    public class BlogService : BlogServiceBase
    {

        public BlogService(ModelStateDictionary modelState)
            : base(modelState, new EntityFrameworkBlogRepository()) { }

        public BlogService(ModelStateDictionary modelState, BlogRepositoryBase blogRepository)
            : base(modelState, blogRepository) { }


        public override IEnumerable<BlogEntry> ListBlogEntries()
        {
            return _blogRepository.ListBlogEntries();
        }


        public override IEnumerable<BlogEntry> ListBlogEntries(int? year, int? month, int? day, string name)
        {
            return _blogRepository.ListBlogEntries(year, month, day, name);
        }


        public override bool CreateBlogEntry(BlogEntry blogEntryToCreate)
        {
            // validation
            if (blogEntryToCreate.Title.Trim().Length == 0)
                _modelState.AddModelError("Title", "Title is required.");
            if (blogEntryToCreate.Title.Length > 500)
                _modelState.AddModelError("Title", "Title is too long.");
            if (blogEntryToCreate.Text.Trim().Length == 0)
                _modelState.AddModelError("Text", "Text is required.");
            if (_modelState.IsValid == false)
                return false;

            // Business Rules
            if (String.IsNullOrEmpty(blogEntryToCreate.Name))
                blogEntryToCreate.Name = blogEntryToCreate.Title;
            blogEntryToCreate.Name = blogEntryToCreate.Name.Replace(" ", "-");
            blogEntryToCreate.Name = Regex.Replace(blogEntryToCreate.Name, "[\"$&+,/:;=?@]", string.Empty);

            // Data access
            _blogRepository.CreateBlogEntry(blogEntryToCreate);
            return true;
        }

    }
}
