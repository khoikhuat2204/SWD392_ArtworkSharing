﻿using DataAccessLayer.Models;

namespace Services.Interface;

public interface ITagService
{
    public bool AddTag(Tag tag);
    public bool UpdateTag(Tag tag);
    public List<Tag> GetAllTags();
    public List<Tag> GetTagsByArtworkId(int artworkId);
    public bool Exists(int tagId);
}